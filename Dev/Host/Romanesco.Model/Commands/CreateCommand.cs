using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Reactive.Bindings;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands
{
	public class CreateCommand
	{
		private readonly Subject<IProjectContext> _onCreateSubject = new Subject<IProjectContext>();
		private readonly IEditorState _currentState;

		public IReadOnlyReactiveProperty<bool> CanExecute { get; }
		public IObservable<IProjectContext> OnCreate => _onCreateSubject;

		internal CreateCommand(IObservable<bool> canExecuteObservable, IEditorState currentState)
		{
			this._currentState = currentState;

			CanExecute = new ReactiveProperty<bool>(canExecuteObservable);
		}
		
		public async Task<IProjectContext?> Execute()
		{
			var project = await _currentState.GetLoadService().CreateAsync();
			if (project is { })
			{
				_onCreateSubject.OnNext(project);
			}

			return project;
		}
	}
}
