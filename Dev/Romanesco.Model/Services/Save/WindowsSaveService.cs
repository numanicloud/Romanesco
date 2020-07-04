using Microsoft.Win32;
using Newtonsoft.Json;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.Serialize;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using Romanesco.Common.Model.Basics;

namespace Romanesco.Model.Services.Save
{
	internal class WindowsSaveService : IProjectSaveService
    {
        private readonly IStateSerializer saveSerializer;
		private readonly ProjectContext context;

        public bool CanSave => true;

        public bool CanExport => true;

        public WindowsSaveService(IStateSerializer saveSerializer,
            ProjectContext context)
        {
            this.saveSerializer = saveSerializer;
			this.context = context;
        }

        public async Task ExportAsync()
        {
            var exporter = context.Exporter;
            var dialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = !exporter.DoExportIntoSingleFile,
                Title = "マスター データを保存",
            };
            var result = dialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                await exporter.ExportAsync(context.Project.Root.RootInstance, dialog.FileName);
            }
        }

        public async Task SaveAsync()
        {
			if (context.Project.DefaultSavePath is string path)
			{
                await SaveToPathAsync(path);
			}
			else
            {
                await SaveAsAsync();
            }
        }

        public async Task SaveAsAsync()
        {
            var project = context.Project;
            var defaultName = "Project.roma";
            if (project.DefaultSavePath is string path)
            {
                defaultName = Path.GetFileName(path);
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
            var data = ProjectConverter.ToData(context.Project, saveSerializer);
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
