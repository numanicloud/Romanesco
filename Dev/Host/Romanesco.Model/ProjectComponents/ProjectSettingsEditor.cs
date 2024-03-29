﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using Microsoft.Win32;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.Reflections;

namespace Romanesco.Model.ProjectComponents
{
	public sealed class ProjectSettingsEditor : IDisposable
	{
		private const string DefaultExporterName = "デフォルト エクスポートAPI";
		private readonly IDataAssemblyRepository assemblyRepo;
		public List<IDisposable> Disposables { get; } = new List<IDisposable>();

		public bool Succeeded { get; set; }
		public ReactiveProperty<string> AssemblyPath { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> ProjectTypeFullName { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string[]> ProjectTypeMenu { get; } = new ReactiveProperty<string[]>();
		public ReactiveProperty<string> ProjectTypeExporterFullName { get; set; } = new ReactiveProperty<string>();
		public ReactiveProperty<string[]> ProjectTypeExporterMenu { get; } = new ReactiveProperty<string[]>();
		public ObservableCollection<string> DependencyProjects { get; } = new ObservableCollection<string>();

		public Assembly Assembly => assemblyRepo.LoadAssemblyFromPath(AssemblyPath.Value);
		public Type? ProjectType => Assembly.GetType(ProjectTypeFullName.Value);
		public Type? ExporterType
		{
			get
			{
				if (ProjectTypeExporterFullName.Value != DefaultExporterName)
				{
					return Assembly.GetType(ProjectTypeExporterFullName.Value);
				}
				else
				{
					return typeof(Services.Export.NewtonsoftJsonProjectTypeExporter);
				}
			}
		}

		public ProjectSettingsEditor(IDataAssemblyRepository assemblyRepo)
		{
			bool IsEditorProjectType(Type t)
			{
				return t.GetCustomAttributesData()
					.Any(x => x.AttributeType.Name == nameof(Annotations.EditorProjectAttribute));
			}

			AssemblyPath.Where(x => x != null).Subscribe(path =>
			{
				var types = assemblyRepo.LoadAssemblyFromPath(path).GetTypes();

				ProjectTypeMenu.Value = types
					.Where(IsEditorProjectType)
					.Select(y => y.FullName ?? "<無効>")
					.ToArray();

				ProjectTypeExporterMenu.Value = types
					.Where(y => typeof(IProjectTypeExporter).IsAssignableFrom(y))
					.Select(y => y.FullName ?? "<無効>")
					.StartsWith(DefaultExporterName)
					.ToArray();

				ProjectTypeExporterFullName.Value = DefaultExporterName;
			}).AddTo(Disposables);
			this.assemblyRepo = assemblyRepo;
		}

		public void OpenAssembly()
		{
			var asm = OpenAssemblyByDialog();
			if (asm != null)
			{
				AssemblyPath.Value = asm;
			}
		}

		public void OpenDependencyProject(int index)
		{
			var asm = OpenProjectByDialog();
			if (string.IsNullOrEmpty(asm))
			{
				return;
			}

			if (index == DependencyProjects.Count)
			{
				DependencyProjects.Add(asm);
			}
			else
			{
				DependencyProjects[index] = asm;
			}
		}

		public void RemoveDependencyProject(string value)
		{
			DependencyProjects.Remove(value);
		}

		private string? OpenProjectByDialog()
		{
			var dialog = new OpenFileDialog()
			{
				Filter = "Romanesco プロジェクト (*.roma)|*.roma",
				Title = "プロジェクトを読み込む"
			};

			var result = dialog.ShowDialog();
			if (result == true)
			{
				return dialog.FileName;
			}
			return null;
		}

		private string? OpenAssemblyByDialog()
		{
			var dialog = new OpenFileDialog()
			{
				Filter = ".NET アセンブリ (*.dll, *.exe)|*.dll;*.exe",
				Title = "アセンブリを読み込む"
			};

			var result = dialog.ShowDialog();
			if (result == true)
			{
				return dialog.FileName;
			}
			return null;
		}

		public void Dispose()
		{
			AssemblyPath.Dispose();
			ProjectTypeFullName.Dispose();
			ProjectTypeMenu.Dispose();
			ProjectTypeExporterFullName.Dispose();
			ProjectTypeExporterMenu.Dispose();
			foreach (var disposable in Disposables)
			{
				disposable.Dispose();
			}
		}
	}
}
