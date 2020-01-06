using System.Threading.Tasks;

namespace Romanesco.Model.Services.Save
{
    interface IProjectSaveService
    {
        Task SaveAsync();
        Task SaveAsAsync();
        void Export();
    }
}
