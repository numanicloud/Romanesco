using System;
using System.Collections.Generic;
using System.Text;
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
	}
}
