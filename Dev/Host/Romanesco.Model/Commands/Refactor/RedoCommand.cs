using System.Threading.Tasks;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands.Refactor
{
	internal class RedoCommand : CommandModelRefactor
	{
		internal override Task Execute(IEditorState state)
		{
			state.GetHistoryService().Redo();
			return Task.CompletedTask;
		}

		internal override void AfterExecute(IEditorState state)
		{
		}
	}
}
