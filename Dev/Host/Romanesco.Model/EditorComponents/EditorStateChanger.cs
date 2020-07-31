using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Subjects;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Model.EditorComponents
{
	internal class EditorStateChanger
	{
		private readonly IModelFactory factory;
		private readonly Subject<EditorState> onChangeSubject = new Subject<EditorState>();
		
		public IObservable<EditorState> OnChange => onChangeSubject;

		public EditorStateChanger(IModelFactory factory)
		{
			this.factory = factory;
		}
		
		public void InitializeState([NotNull]out EditorState store)
		{
			var state = factory.ResolveEmptyEditorStateAsTransient();
			store = state;
			onChangeSubject.OnNext(state);
		}

		public void ChangeState(EditorState state)
		{
			onChangeSubject.OnNext(state);
		}
	}
}
