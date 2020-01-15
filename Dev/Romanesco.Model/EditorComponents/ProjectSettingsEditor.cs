using Microsoft.Win32;
using Reactive.Bindings;
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
        public bool Succeeded { get; set; }
        public ReactiveProperty<string> AssemblyPath { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ProjectTypeFullName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string[]> ProjectTypeMenu { get; } = new ReactiveProperty<string[]>();

        public Assembly Assembly => Assembly.LoadFrom(AssemblyPath.Value);
        public Type ProjectType => Assembly.GetType(ProjectTypeFullName.Value);

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

                ProjectTypeMenu.Value = assembly.GetTypes()
                    .Where(y => y.GetCustomAttribute<Annotations.EditorProjectAttribute>() != null)
                    .Select(y => y.FullName)
                    .ToArray();
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
