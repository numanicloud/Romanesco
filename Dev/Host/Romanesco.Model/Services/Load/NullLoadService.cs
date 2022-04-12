using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;

namespace Romanesco.Model.Services.Load
{
	internal class NullLoadService : IProjectLoadService
    {
        public bool CanCreate => false;

        public bool CanOpen => false;

        public async Task<IProjectContext?> CreateAsync()
        {
            return null;
        }

        public async Task<IProjectContext?> OpenAsync()
        {
            return null;
        }
    }
}
