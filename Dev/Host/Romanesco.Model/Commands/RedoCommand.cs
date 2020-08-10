using Reactive.Bindings;
using System;
using System.Reactive;
using System.Reactive.Subjects;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands
{
	public class RedoCommand : ICommandModel
	{
		private readonly Subject<Unit> _onExecuteSubject = new Subject<Unit>();
		private readonly IEditorState _currentState;

		public IReadOnlyReactiveProperty<bool> CanExecute { get; }
		public IObservable<Unit> OnExecuted => _onExecuteSubject;

		internal RedoCommand(IObservable<bool> canExecuteObservable, IEditorState currentState)
		{
			this._currentState = currentState;
			CanExecute = new ReactiveProperty<bool>(canExecuteObservable);
		}
		
		public void Execute()
		{
			_currentState.GetHistoryService().Redo();
			_onExecuteSubject.OnNext(Unit.Default);
		}
	}
}
