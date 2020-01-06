using Romanesco.Model.ProjectComponents;
using System.Threading.Tasks;

namespace Romanesco.Model.Services.Load
{
    interface IProjectLoadService
    {
        Project Create();
        Task<Project> OpenAsync();
    }
}
