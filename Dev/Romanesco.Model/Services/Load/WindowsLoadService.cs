using Microsoft.Win32;
using Newtonsoft.Json;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Romanesco.Model.Services.Load
{
	class WindowsLoadService : IProjectLoadService
	{
		private readonly IProjectSettingProvider projectSettingProvider;
		private readonly IStateDeserializer deserializer;
		private readonly IDataAssemblyRepository assemblyRepo;
		private readonly IServiceLocator serviceLocator;
		private readonly ObjectInterpreter interpreter;

		public WindowsLoadService(IProjectSettingProvider projectSettingProvider,
			IStateDeserializer deserializer,
			IDataAssemblyRepository assemblyRepo,
			IServiceLocator serviceLocator,
			ObjectInterpreter interpreter)
		{
			this.projectSettingProvider = projectSettingProvider;
			this.deserializer = deserializer;
			this.assemblyRepo = assemblyRepo;
			this.serviceLocator = serviceLocator;
			this.interpreter = interpreter;
		}

		public bool CanCreate => true;

		public bool CanOpen => true;

		public async Task<ProjectContext?> CreateAsync()
		{
			return await ResetProject(CreateInternalAsync);
		}

		public async Task<ProjectContext?> OpenAsync()
		{
			return await ResetProject(OpenInternalAsync);
		}

		private async Task<ProjectContext?> ResetProject(Func<Task<Project?>> generator)
		{
			var project = await generator();
			if (project != null)
			{
				if (Activator.CreateInstance(project.Settings.ExporterType) is IProjectTypeExporter exporter)
				{
					return new ProjectContext(project, exporter);
				}
				else
				{
					throw new InvalidOperationException($"{project.Settings.ExporterType}をインスタンス化できませんでした。");
				}
			}
			else
			{
				return null;
			}
		}

		private async Task<Project?> CreateInternalAsync()
		{
			var editor = serviceLocator.GetService<ProjectSettingsEditor>();
			var settings = projectSettingProvider.InputCreateSettings(editor);

			if (settings != null)
			{
				if (assemblyRepo.CreateInstance(settings.ProjectType) is object instance)
				{
					return await ProjectConverter.FromInstanceAsync(settings, deserializer, interpreter, instance);
				}
				else
				{
					throw new InvalidOperationException($"プロジェクト型 {settings.ProjectType} のインスタンスを作成できませんでした。");
				}
			}
			return null;
		}

		private async Task<Project?> OpenInternalAsync()
		{
			var dialog = new OpenFileDialog()
			{
				Filter = "マスター プロジェクト (*.roma)|*.roma",
				Title = "マスター プロジェクトを読み込む"
			};

			var result = dialog.ShowDialog();
			if (result == true)
			{
				using var file = File.OpenRead(dialog.FileName);
				using var reader = new StreamReader(file);

				var json = await reader.ReadToEndAsync();
				var data = JsonConvert.DeserializeObject<ProjectData>(json);
				var project = await ProjectConverter.FromDataAsync(data, deserializer, interpreter);
				project.DefaultSavePath = dialog.FileName;
				return project;
			}
			else
			{
				return null;
			}
		}
	}
}
