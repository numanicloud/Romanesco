using Romanesco.Common.Model.Helpers;
using System.Threading.Tasks;
using Romanesco.Model.Interfaces;

namespace Romanesco.ViewModel.Commands
{
	internal class ExportCommandViewModel : CommandViewModel
	{
		private readonly ICommandAvailabilityPublisher model;
		private readonly BooleanUsingScopeSource commandExecution;

		public ExportCommandViewModel(ICommandAvailabilityPublisher model, BooleanUsingScopeSource commandExecution)
			: base(model.CanExport, commandExecution)
		{
			this.model = model;
			this.commandExecution = commandExecution;
		}

		public override void Execute(object? parameter) => ExportAsync().Forget();

		private async Task ExportAsync()
		{
			using (commandExecution.Create())
			{
				await model.ExportAsync();
			}
		}
	}
}
