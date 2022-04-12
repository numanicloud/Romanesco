using System.Windows.Input;
using Romanesco.Common.Model.Helpers;
using Romanesco.Model.Interfaces;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Commands
{
	internal class CommandManagerViewModel
	{
		public ICommand Create { get; }
		public ICommand Open { get; }
		public ICommand Save { get; }
		public ICommand SaveAs { get; }
		public ICommand Export { get; }
		public ICommand Undo { get; }
		public ICommand Redo { get; }

		public CommandManagerViewModel(ICommandAvailabilityPublisher model,
			RootViewModel root, IViewModelInterpreter interpreter)
		{
			var commandExecution = new BooleanUsingScopeSource();
			Create = new CreateCommandViewModel(model, root.Fields, interpreter, commandExecution);
			Open = new OpenCommandViewModel(model, commandExecution, root.Fields, interpreter);
			Save = new SaveCommandViewModel(model, commandExecution);
			SaveAs = new SaveAsCommandViewModel(model, commandExecution);
			Export = new ExportCommandViewModel(model, commandExecution);
			Undo = new UndoCommandViewModel(model, commandExecution);
			Redo = new RedoCommandViewModel(model, commandExecution);
		}
	}
}
