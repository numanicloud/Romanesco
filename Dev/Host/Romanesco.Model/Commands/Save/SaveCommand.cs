using Reactive.Bindings;
using System;
using System.Threading.Tasks;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands
{
	public class SaveCommand : ICommandModel
	{
		private readonly IEditorState currentState;

		public IReadOnlyReactiveProperty<bool> CanExecute { get; }

		internal SaveCommand(IObservable<bool> canExecuteObservable, IEditorState currentState)
		{
			this.currentState = currentState;
			CanExecute = new ReactiveProperty<bool>(canExecuteObservable);
		}
		
		public async Task Execute()
		{
			await currentState.GetSaveService().SaveAsync();
			currentState.OnSave();
		}
	}
}
