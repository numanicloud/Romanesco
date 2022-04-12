using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Reactive.Bindings;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands
{
	public class OpenCommand : ICommandModel
	{
		private readonly Subject<IProjectContext> _onExecutedSubject = new Subject<IProjectContext>();
		private readonly IEditorState _currentState;

		public IReadOnlyReactiveProperty<bool> CanExecute { get; }
		public IObservable<IProjectContext> OnExecuted => _onExecutedSubject;

		internal OpenCommand(IObservable<bool> canExecuteObservable, IEditorState currentState)
		{
			this._currentState = currentState;
			CanExecute = new ReactiveProperty<bool>(canExecuteObservable);
		}
		
		public async Task<IProjectContext?> Execute()
		{
			var project = await _currentState.GetLoadService().OpenAsync();
			if (project is { })
			{
				_onExecutedSubject.OnNext(project);
				_currentState.OnOpen(project);
			}

			return project;
		}
	}
}
