using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.EditorComponents
{
	internal interface IEditorStateChanger
	{
		IObservable<IEditorState> OnChange { get; }
		void ChangeState(IEditorState state);
		IEditorState GetInitialState();
	}
}
