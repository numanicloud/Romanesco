using System;
using System.Reactive.Subjects;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Model.EditorComponents
{
	internal class EditorStateChanger : IEditorStateChanger
	{
		private readonly IModelFactory _factory;
		private readonly Subject<IEditorState> _onChangeSubject = new Subject<IEditorState>();
		private IEditorState _currentState;
		
		public IObservable<IEditorState> OnChange => _onChangeSubject;

		public EditorStateChanger(IModelFactory factory)
		{
			this._factory = factory;
			_currentState = GetInitialState();
		}

		public IEditorState GetInitialState()
		{
			return _factory.ResolveEmptyEditorStateAsTransient();
		}

		public IEditorState GetCurrent()
		{
			return _currentState;
		}

		public void ChangeState(IEditorState state)
		{
			_onChangeSubject.OnNext(state);
			_currentState = state;
		}
	}
}
