using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.EditorComponents
{
	internal interface IEditorStateChanger
	{
		IObservable<EditorState> OnChange { get; }
		void ChangeState(EditorState state);
		EditorState GetInitialState();
	}
}
