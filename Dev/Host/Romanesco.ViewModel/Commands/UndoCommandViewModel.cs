using Romanesco.Common.Model.Helpers;
using Romanesco.Model.Interfaces;

namespace Romanesco.ViewModel.Commands
{
	internal class UndoCommandViewModel : CommandViewModel
	{
		private readonly ICommandAvailabilityPublisher model;

		public UndoCommandViewModel(ICommandAvailabilityPublisher model, BooleanUsingScopeSource commandExecution)
			: base(model.CanUndo, commandExecution)
		{
			this.model = model;
		}

		public override void Execute(object? parameter) => model.Undo();
	}
}
