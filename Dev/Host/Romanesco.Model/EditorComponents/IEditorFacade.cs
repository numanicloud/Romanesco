using System;
using System.Collections.Generic;
using Romanesco.Model.Interfaces;

namespace Romanesco.Model.EditorComponents
{
	public interface IEditorFacade
    {
        List<IDisposable> Disposables { get; }
        ICommandAvailabilityPublisher CommandAvailabilityPublisher { get; }
    }
}
