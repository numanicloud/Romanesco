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
		private readonly Subject<EditorState> onChangeSubject = new Subject<EditorState>();
		
		public IObservable<EditorState> OnChange => onChangeSubject;

		public EditorStateChanger(IModelFactory factory)
		{
			this.factory = factory;
		}

		public EditorState GetInitialState()
		{
			return factory.ResolveEmptyEditorStateAsTransient();
		}

		public void ChangeState(EditorState state)
		{
			onChangeSubject.OnNext(state);
		}
	}
}
