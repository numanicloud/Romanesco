using System;
using Romanesco.Model.EditorComponents;

namespace Romanesco.Model.Commands
{
	internal class CommandAvailability
	{
		public IObserver<(EditorCommandType, bool)> Observer { get; }

		public CommandAvailability(IObserver<(EditorCommandType, bool)> observer)
		{
			Observer = observer;
		}

		public void UpdateCanExecute(EditorCommandType commandType, bool canExecute)
		{
			Observer.OnNext((commandType, canExecute));
		}
	}
}
