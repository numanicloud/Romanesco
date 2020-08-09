using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.Interfaces;

namespace Romanesco.Model.EditorComponents
{
	public interface IEditorFacade
    {
        List<IDisposable> Disposables { get; }
        ICommandAvailabilityPublisher CommandAvailabilityPublisher { get; }
        Task<IProjectContext?> CreateAsync();
        Task<IProjectContext?> OpenAsync();
        Task SaveAsAsync();
    }
}
