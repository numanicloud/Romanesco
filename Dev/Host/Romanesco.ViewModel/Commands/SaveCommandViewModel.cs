using System.Threading.Tasks;
using Romanesco.Common.Model.Helpers;
using Romanesco.Model.Interfaces;

namespace Romanesco.ViewModel.Commands
{
	internal class SaveCommandViewModel : CommandViewModel
	{
		private readonly ICommandAvailabilityPublisher model;
		private readonly BooleanUsingScopeSource commandExecution;

		public SaveCommandViewModel(ICommandAvailabilityPublisher model,
			BooleanUsingScopeSource commandExecution)
			: base(model.CanSave, commandExecution)
		{
			this.model = model;
			this.commandExecution = commandExecution;
		}

		public override void Execute(object? parameter) => SaveAsync().Forget();

		private async Task SaveAsync()
		{
			using (commandExecution.Create())
			{
				await model.SaveAsync();
			}
		}
	}
}
