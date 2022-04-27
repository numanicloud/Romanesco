using System.Threading.Tasks;
using Romanesco.Model.States;

namespace Romanesco.Model.Commands
{
	internal class RedoRomanescoCommand : RomanescoCommand
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
