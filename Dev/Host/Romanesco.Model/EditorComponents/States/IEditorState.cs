using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;

namespace Romanesco.Model.EditorComponents.States
{
	internal interface IEditorState
	{
		string Title { get; }

		void OnCreate(IProjectContext project);
		void OnOpen(IProjectContext project);

		Task<IProjectContext?> CreateAsync();
		Task<IProjectContext?> OpenAsync();
		void NotifyEdit(CommandAvailability commandAvailability);
		Task SaveAsync();
		Task SaveAsAsync();
		Task ExportAsync();

		// このへんはCommandAvailabilityの責務かも？
		// あるいはEditor側がCommandAvailabilityを使うようにするとよいか？
		// EditorとEditorStateの両方に、CommandAvailabilityを注入するという手がある
		// その場合EditorViewModelにもCommandAvailabilityを注入すれば実装可能そう
		void Undo(CommandAvailability availability);
		void Redo(CommandAvailability availability);
		void UpdateCanExecute(CommandAvailability availability);
	}
}
