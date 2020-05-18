using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.ProjectComponents;
using System.Threading.Tasks;

namespace Romanesco.Model.Services.Load
{
    interface IProjectLoadService
    {
        bool CanCreate { get; }
        bool CanOpen { get; }
        Task<ProjectContext?> CreateAsync();
        Task<ProjectContext?> OpenAsync();
    }
}
