using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Subjects;
using System.Text;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Model.EditorComponents
{
	internal class EditorStateChanger2
	{
		private readonly IModelFactory factory;
		private readonly Subject<EditorState> onChangeSubject = new Subject<EditorState>();
		
		public IObservable<EditorState> OnChange => onChangeSubject;

		public EditorStateChanger2(IModelFactory factory)
		{
			this.factory = factory;
		}
		
		public void InitializeState([NotNull]ref EditorState store)
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
