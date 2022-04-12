using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Reactive.Bindings;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.Interfaces;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Commands
{
	internal class OpenCommandViewModel : CommandViewModel
	{
		private readonly ICommandAvailabilityPublisher model;
		private readonly BooleanUsingScopeSource commandExecution;
		private readonly ReactiveProperty<IStateViewModel[]> roots;
		private readonly IViewModelInterpreter interpreter;

		public OpenCommandViewModel(ICommandAvailabilityPublisher model, BooleanUsingScopeSource commandExecution,
			ReactiveProperty<IStateViewModel[]> roots, IViewModelInterpreter interpreter)
			: base(model.CanOpen, commandExecution)
		{
			this.model = model;
			this.commandExecution = commandExecution;
			this.roots = roots;
			this.interpreter = interpreter;
		}

		public override void Execute(object? parameter) => OpenAsync().Forget();

		private async Task OpenAsync()
		{
			using (commandExecution.Create())
			{
				var projectContext = await model.OpenAsync();
				if (projectContext == null)
				{
					return;
				}

				roots.Value = projectContext.Project.Root.States
					.Select(s => interpreter.InterpretAsViewModel(s))
					.ToArray();
			}
		}
	}
}
