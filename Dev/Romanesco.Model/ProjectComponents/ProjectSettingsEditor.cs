using Microsoft.Win32;
using Reactive.Bindings;
using Romanesco.Common.Model;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;
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

        public Assembly Assembly => Assembly.LoadFrom(AssemblyPath.Value);
        public Type ProjectType => Assembly.GetType(ProjectTypeFullName.Value);
        public Type ExporterType
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
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFrom(x);
                }
                catch (Exception)
                {
                    return;
                }

                var types = assembly.GetTypes();

                ProjectTypeMenu.Value = types
                    .Where(y => y.GetCustomAttribute<Annotations.EditorProjectAttribute>() != null)
                    .Select(y => y.FullName)
                    .ToArray();

                ProjectTypeExporterMenu.Value = types
                    .Where(y => typeof(IProjectTypeExporter).IsAssignableFrom(y))
                    .Select(y => y.FullName)
                    .StartsWith(DefaultExporterName)
                    .ToArray();

                ProjectTypeExporterFullName.Value = DefaultExporterName;
            });
        }

        public void OpenAssembly()
        {
            var dialog = new OpenFileDialog()
            {
                Filter = ".NET アセンブリ (*.dll, *.exe)|*.dll;*.exe",
                Title = "アセンブリを読み込む"
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                AssemblyPath.Value = dialog.FileName;
            }
        }
    }
}
