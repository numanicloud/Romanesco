using Romanesco.Common.Model.Helpers;
using System.Threading.Tasks;
using Romanesco.Model.Interfaces;

namespace Romanesco.ViewModel.Commands
{
	internal class SaveAsCommandViewModel : CommandViewModel
	{
		private readonly ICommandAvailabilityPublisher model;
		private readonly BooleanUsingScopeSource commandExecution;

		public SaveAsCommandViewModel(ICommandAvailabilityPublisher model, BooleanUsingScopeSource commandExecution)
			: base(model.CanSaveAs, commandExecution)
		{
			this.model = model;
			this.commandExecution = commandExecution;
		}

		public override void Execute(object? parameter) => SaveAsAsync().Wait();

		private async Task SaveAsAsync()
		{
			using (commandExecution.Create())
			{
				await model.SaveAsAsync();
			}
		}
	}
}
