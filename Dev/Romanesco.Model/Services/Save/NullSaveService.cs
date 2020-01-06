using System.Threading.Tasks;

namespace Romanesco.Model.Services.Save
{
    class NullSaveService : IProjectSaveService
    {
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
