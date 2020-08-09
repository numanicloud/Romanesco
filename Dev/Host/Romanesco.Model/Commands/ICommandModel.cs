using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;

namespace Romanesco.Model.Commands
{
	interface ICommandModel
	{
		IReadOnlyReactiveProperty<bool> CanExecute { get; }
	}
}
