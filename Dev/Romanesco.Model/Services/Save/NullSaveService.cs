using System.Threading.Tasks;

namespace Romanesco.Model.Services.Save
{
    class NullSaveService : IProjectSaveService
    {
        public bool CanSave => false;

        public bool CanExport => false;

        public void Export()
        {
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
