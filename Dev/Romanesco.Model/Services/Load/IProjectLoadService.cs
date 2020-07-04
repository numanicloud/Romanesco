using Romanesco.Common.Model.Basics;
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
