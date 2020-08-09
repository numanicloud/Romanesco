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
	internal class CreateCommandViewModel : CommandViewModel
	{
		private readonly ICommandAvailabilityPublisher model;
		private readonly IViewModelInterpreter interpreter;
		private readonly ReactiveProperty<IStateViewModel[]> roots;
		private readonly BooleanUsingScopeSource commandExecution;

		public CreateCommandViewModel(ICommandAvailabilityPublisher model,
			ReactiveProperty<IStateViewModel[]> roots,
			IViewModelInterpreter interpreter,
			BooleanUsingScopeSource commandExecution)
			: base(model.CanCreate, commandExecution)
		{
			this.model = model;
			this.interpreter = interpreter;
			this.commandExecution = commandExecution;
			this.roots = roots;
		}

		public override void Execute(object? parameter) => CreateAsync().Forget();

		private async Task CreateAsync()
		{
			// 実行をロックする機能は IProjectLoadService 側に入れるべきかも
			using (commandExecution.Create())
			{
				var projectContext = await model.CreateAsync();
				if (projectContext != null)
				{
					roots.Value = projectContext.Project.Root.States
						.Select(s => interpreter.InterpretAsViewModel(s))
						.ToArray();
				}
			}
		}
	}
}
