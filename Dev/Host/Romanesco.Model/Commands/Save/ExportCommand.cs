using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands
{
	public class ExportCommand : ICommandModel
	{
		private readonly IEditorState _currentState;

		public IReadOnlyReactiveProperty<bool> CanExecute { get; }

		internal ExportCommand(IObservable<bool> canExecuteObservable, IEditorState currentState)
		{
			this._currentState = currentState;
			CanExecute = new ReactiveProperty<bool>(canExecuteObservable);
		}
		
		public async Task Execute()
		{
			await _currentState.GetSaveService().ExportAsync();
		}
	}
}
