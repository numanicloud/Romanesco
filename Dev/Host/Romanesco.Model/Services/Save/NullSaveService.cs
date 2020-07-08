using System.Threading.Tasks;

namespace Romanesco.Model.Services.Save
{
	internal class NullSaveService : IProjectSaveService
    {
        public bool CanSave => false;

        public bool CanExport => false;

        public Task ExportAsync()
        {
            return Task.CompletedTask;
        }

        public Task SaveAsAsync()
        {
            return Task.CompletedTask;
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }
}
