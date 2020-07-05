using Romanesco.Common.Model.Basics;
using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;

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
