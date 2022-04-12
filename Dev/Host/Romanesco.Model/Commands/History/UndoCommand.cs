using Reactive.Bindings;
using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands
{
	public class UndoCommand : ICommandModel
	{
		private readonly Subject<Unit> _onExecutedSubject = new Subject<Unit>();
		private readonly IEditorState _currentState;

		public IReadOnlyReactiveProperty<bool> CanExecute { get; }
		public IObservable<Unit> OnExecuted => _onExecutedSubject;

		internal UndoCommand(IObservable<bool> canExecuteObservable, IEditorState currentState)
		{
			this._currentState = currentState;
			CanExecute = new ReactiveProperty<bool>(canExecuteObservable);
		}
		
		public void Execute()
		{
			_currentState.GetHistoryService().Undo();
			_onExecutedSubject.OnNext(Unit.Default);
		}
	}
}
