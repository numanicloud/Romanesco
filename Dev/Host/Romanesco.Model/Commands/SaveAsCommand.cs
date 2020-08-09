using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands
{
	public class SaveAsCommand : ICommandModel
	{
		private readonly Subject<Unit> _onSaveAsSubject = new Subject<Unit>();
		private readonly IEditorState _currentState;

		public IReadOnlyReactiveProperty<bool> CanExecute { get; }
		public IObservable<Unit> OnSaveAs => _onSaveAsSubject;

		internal SaveAsCommand(IObservable<bool> canExecuteObservable, IEditorState currentState)
		{
			this._currentState = currentState;
			CanExecute = new ReactiveProperty<bool>(canExecuteObservable);
		}
		
		public async Task Execute()
		{
			await _currentState.GetSaveService().SaveAsAsync();
			_currentState.OnSaveAs();
			_onSaveAsSubject.OnNext(Unit.Default);
		}
	}
}
