using Microsoft.Win32;
using Newtonsoft.Json;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.Export;
using Romanesco.Model.Services.Serialize;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Model.Services.Save
{
    class WindowsSaveService : IProjectSaveService
    {
        private readonly Project project;
        private readonly IStateSerializer saveSerializer;
        private readonly IProjectTypeExporter exporter;

        public bool CanSave => true;

        public bool CanExport => true;

        public WindowsSaveService(Project project,
            IStateSerializer saveSerializer,
            IProjectTypeExporter exporter)
        {
            this.project = project;
            this.saveSerializer = saveSerializer;
            this.exporter = exporter;
        }

        public async Task ExportAsync()
        {
            var dialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = !exporter.DoExportIntoSingleFile,
                Title = "マスター データを保存",
            };
            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                await exporter.ExportAsync(project.Root.RootInstance, dialog.FileName);
            }
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
