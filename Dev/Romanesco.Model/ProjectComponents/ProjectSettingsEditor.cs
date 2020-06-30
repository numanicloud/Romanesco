using Microsoft.Win32;
using Reactive.Bindings;
using Romanesco.Common.Model;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.ProjectComponents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;

namespace Romanesco.Model.EditorComponents
{
    public class ProjectSettingsEditor
    {
        private const string DefaultExporterName = "デフォルト エクスポートAPI";

        public bool Succeeded { get; set; }
        public ReactiveProperty<string> AssemblyPath { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ProjectTypeFullName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string[]> ProjectTypeMenu { get; } = new ReactiveProperty<string[]>();
        public ReactiveProperty<string> ProjectTypeExporterFullName { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string[]> ProjectTypeExporterMenu { get; } = new ReactiveProperty<string[]>();
        public ObservableCollection<string> DependencyProjects { get; } = new ObservableCollection<string>();

        public Assembly Assembly
		{
            get
			{
                var loader = new MyAssemblyLoadContext(AssemblyPath.Value);
                return loader.LoadFromAssemblyPath(AssemblyPath.Value);
            }
		}
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

        public ProjectSettingsEditor()
        {
            AssemblyPath.Where(x => x != null).Subscribe(x =>
            {
                var loader = new MyAssemblyLoadContext(x);

                Assembly assembly;
                try
                {
                    assembly = loader.LoadFromAssemblyPath(x);
                }
                catch (Exception)
                {
                    return;
                }

                try
                {
                var types = assembly.GetTypes();

                ProjectTypeMenu.Value = types
                    .Where(y => y.GetCustomAttribute<Annotations.EditorProjectAttribute>() != null)
                    .Select(y => y.FullName ?? "<無効>")
                    .ToArray();

                ProjectTypeExporterMenu.Value = types
                    .Where(y => typeof(IProjectTypeExporter).IsAssignableFrom(y))
                    .Select(y => y.FullName ?? "<無効>")
                    .StartsWith(DefaultExporterName)
                    .ToArray();

                ProjectTypeExporterFullName.Value = DefaultExporterName;

                }
                catch (Exception)
                {
                    throw;
                }
            });
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
    }
}
