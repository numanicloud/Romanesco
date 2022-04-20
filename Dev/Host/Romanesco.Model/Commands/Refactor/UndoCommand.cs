using System.Threading.Tasks;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands.Refactor;

internal class UndoCommand : CommandModelRefactor
{
	internal override async Task Execute(IEditorState state)
	{
		state.GetHistoryService().Undo();
	}

	internal override void AfterExecute(IEditorState state)
	{
	}
}