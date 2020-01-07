using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.ProjectComponents;
using System.Threading.Tasks;

namespace Romanesco.Model.Services.Load
{
    class NullLoadService : IProjectLoadService
    {
        public bool CanCreate => false;

        public bool CanOpen => false;

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
