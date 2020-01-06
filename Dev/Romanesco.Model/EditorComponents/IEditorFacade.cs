using Romanesco.Model.ProjectComponents;
using System.Threading.Tasks;

namespace Romanesco.Model.EditorComponents
{
    public interface IEditorFacade
    {
        Project Create();
        Task<Project> OpenAsync();
        Task SaveAsync();
        Task SaveAsAsync();
        void Export();
        void Undo();
        void Redo();
    }
}
