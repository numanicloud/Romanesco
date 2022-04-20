using System;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.EditorComponents
{
	internal interface IEditorStateChanger
	{
		IObservable<IEditorState> OnChange { get; }
		void ChangeState(IEditorState state);
		IEditorState GetCurrent();
	}
}
