using System.Threading.Tasks;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands.Refactor
{
	internal class ExportCommand : CommandModelRefactor
	{
		internal override async Task Execute(IEditorState state)
		{
			await state.GetSaveService().ExportAsync();
		}

		internal override void AfterExecute(IEditorState state)
		{
		}
	}
}
