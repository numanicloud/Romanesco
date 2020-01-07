using Microsoft.Win32;
using Newtonsoft.Json;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.Serialize;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Romanesco.Model.Services.Save
{
    class WindowsSaveService : IProjectSaveService
    {
        private readonly Project project;
        private readonly IStateSerializer saveSerializer;

        public WindowsSaveService(Project project, IStateSerializer saveSerializer)
        {
            this.project = project;
            this.saveSerializer = saveSerializer;
        }

        public void Export()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            if (project.DefaultSavePath == null)
            {
                await SaveAsAsync();
                return;
            }

            await SaveToPathAsync(project.DefaultSavePath);
        }

        public async Task SaveAsAsync()
        {
            var defaultName = "Project.roma";
            if (project.DefaultSavePath != null)
            {
                defaultName = Path.GetFileName(project.DefaultSavePath);
            }

            var dialog = new SaveFileDialog()
            {
                FileName = defaultName,
                Filter = "マスター プロジェクト (*.roma)|*.roma",
                Title = "マスター プロジェクトを保存"
            };
            var result = dialog.ShowDialog();

            if (result == true)
            {
                await SaveToPathAsync(dialog.FileName);
                project.DefaultSavePath = dialog.FileName;
            }
        }

        private async Task SaveToPathAsync(string path)
        {
            var data = ProjectConverter.ToData(project, saveSerializer);
            var json = JsonConvert.SerializeObject(data);

            using (var file = File.Create(path))
            {
                using (var writer = new StreamWriter(file))
                {
                    await writer.WriteAsync(json);
                }
            }
        }
    }
}
