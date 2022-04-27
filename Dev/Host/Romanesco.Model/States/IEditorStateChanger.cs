using System;

namespace Romanesco.Model.States
{
	internal interface IEditorStateChanger
	{
		IObservable<IEditorState> OnChange { get; }
		void ChangeState(IEditorState state);
		IEditorState GetCurrent();
	}
}
