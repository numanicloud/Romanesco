using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.ProjectComponents;
using System.Threading.Tasks;

namespace Romanesco.Model.EditorComponents
{
    public interface IEditorFacade
    {
        ProjectContext Create();
        Task<ProjectContext> OpenAsync();
        Task SaveAsync();
        Task SaveAsAsync();
        void Export();
        void Undo();
        void Redo();
    }
}
