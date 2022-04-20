using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands.Refactor;

internal interface IStateChanger
{
	IEditorState GetCurrent();
	void ChangeState(IEditorState state);
}
