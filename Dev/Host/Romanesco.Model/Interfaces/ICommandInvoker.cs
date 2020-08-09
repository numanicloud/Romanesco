using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Interfaces
{
	public interface ICommandInvoker
	{
		Task<IProjectContext?> CreateAsync();

		Task<IProjectContext?> OpenAsync();

		Task SaveAsync();

		Task SaveAsAsync();

		Task ExportAsync();

		void Undo();

		void Redo();
	}
}
