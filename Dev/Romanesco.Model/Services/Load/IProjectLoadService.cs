using Romanesco.Common.Model.Basics;
using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;

namespace Romanesco.Model.Services.Load
{
	internal interface IProjectLoadService
    {
        bool CanCreate { get; }
        bool CanOpen { get; }
        Task<ProjectContext?> CreateAsync();
        Task<ProjectContext?> OpenAsync();
    }
}
