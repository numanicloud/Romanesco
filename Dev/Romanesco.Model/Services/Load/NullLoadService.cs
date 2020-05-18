using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.ProjectComponents;
using System.Threading.Tasks;

namespace Romanesco.Model.Services.Load
{
    class NullLoadService : IProjectLoadService
    {
        public bool CanCreate => false;

        public bool CanOpen => false;

        public async Task<ProjectContext?> CreateAsync()
        {
            return null;
        }

        public async Task<ProjectContext?> OpenAsync()
        {
            return null;
        }
    }
}
