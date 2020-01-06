using Microsoft.Win32;
using Newtonsoft.Json;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.Serialize;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Romanesco.Model.Services.Load
{
    class WindowsLoadService : IProjectLoadService
    {
        private readonly EditorContext context;
        private readonly IStateDeserializer deserializer;

        public WindowsLoadService(EditorContext context, IStateDeserializer deserializer)
        {
            this.context = context;
            this.deserializer = deserializer;
        }

        public Project Create()
        {
            var settings = context.SettingProvider.GetSettings();
            var instance = Activator.CreateInstance(settings.ProjectType);
            return Project.FromInstance(settings, context.Interpreter, instance);
        }

        public async Task<Project> OpenAsync()
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "マスター プロジェクト (*.roma)|*.roma",
                Title = "マスター プロジェクトを読み込む"
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                using (var file = File.OpenRead(dialog.FileName))
                {
                    using (var reader = new StreamReader(file))
                    {
                        var json = await reader.ReadToEndAsync();
                        var data = JsonConvert.DeserializeObject<ProjectData>(json);
                        var project = Project.FromData(data, deserializer, context.Interpreter);
                        project.DefaultSavePath = dialog.FileName;
                        return project;
                    }
                }
            }
            else
            {
                return null;
            }
        }
    }
}
