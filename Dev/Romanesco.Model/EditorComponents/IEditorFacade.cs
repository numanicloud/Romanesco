using Romanesco.Model.ProjectComponents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Romanesco.Model
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
