using Romanesco.Common.Model.Helpers;
using Romanesco.Model.Interfaces;

namespace Romanesco.ViewModel.Commands
{
	internal class RedoCommandViewModel : CommandViewModel
	{
		private readonly ICommandAvailabilityPublisher model;

		public RedoCommandViewModel(ICommandAvailabilityPublisher model, BooleanUsingScopeSource commandExecution)
			: base(model.CanRedo, commandExecution)
		{
			this.model = model;
		}

		public override void Execute(object? parameter) => model.Redo();
	}
}
