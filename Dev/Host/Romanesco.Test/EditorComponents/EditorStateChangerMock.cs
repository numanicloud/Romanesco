using System;
using Reactive.Bindings;
using Romanesco.Model.States;

namespace Romanesco.Test.EditorComponents
{
	internal class EditorStateChangerMock : IEditorStateChanger
	{
		private readonly ReactiveProperty<IEditorState> _state = new();

		public IObservable<IEditorState> OnChange => _state;

		public void ChangeState(IEditorState state) => _state.Value = state;

		public IEditorState GetCurrent() => _state.Value;
	}
}
