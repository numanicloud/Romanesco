using Reactive.Bindings;
using Romanesco.Common.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Model.Interfaces;

namespace Romanesco.ViewModel.Commands
{
	internal class SaveAsCommandViewModel : CommandViewModel
	{
		public SaveAsCommandViewModel(ICommandAvailabilityPublisher model, BooleanUsingScopeSource commandExecution)
			: base(model.CanSaveAs, commandExecution)
		{
		}

		public override void Execute(object? parameter)
		{
			throw new NotImplementedException();
		}
	}
}
