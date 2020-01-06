using Romanesco.Model.ProjectComponents;
using System.Threading.Tasks;

namespace Romanesco.Model.Services.Load
{
    class NullLoadService : IProjectLoadService
    {
        public Project Create()
        {
            return null;
        }

        public Task<Project> OpenAsync()
        {
            return null;
        }
    }
}
