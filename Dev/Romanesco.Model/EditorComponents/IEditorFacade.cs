using Romanesco.Common.Model.Basics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Romanesco.Model.EditorComponents
{
	public interface IEditorFacade
    {
        List<IDisposable> Disposables { get; }
        IObservable<(EditorCommandType command, bool canExecute)> CanExecuteObservable { get; }
        Task<ProjectContext?> CreateAsync();
        Task<ProjectContext?> OpenAsync();
        Task SaveAsync();
        Task SaveAsAsync();
        Task ExportAsync();
        void Undo();
        void Redo();
    }
}
