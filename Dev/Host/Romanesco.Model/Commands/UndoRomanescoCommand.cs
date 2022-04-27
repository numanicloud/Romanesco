using System.Threading.Tasks;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands;

internal class UndoRomanescoCommand : RomanescoCommand
{
	internal override async Task Execute(IEditorState state)
	{
		state.GetHistoryService().Undo();
	}

	internal override void AfterExecute(IEditorState state)
	{
	}
}