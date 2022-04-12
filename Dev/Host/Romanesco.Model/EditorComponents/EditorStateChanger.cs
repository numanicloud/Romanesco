using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Subjects;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Model.EditorComponents
{
	internal class EditorStateChanger : IEditorStateChanger
	{
		private readonly IModelFactory factory;
		private readonly Subject<IEditorState> onChangeSubject = new Subject<IEditorState>();
		
		public IObservable<IEditorState> OnChange => onChangeSubject;

		public EditorStateChanger(IModelFactory factory)
		{
			this.factory = factory;
		}

		public IEditorState GetInitialState()
		{
			return factory.ResolveEmptyEditorStateAsTransient();
		}

		public void ChangeState(IEditorState state)
		{
			onChangeSubject.OnNext(state);
		}
	}
}
