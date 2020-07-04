using System.Threading.Tasks;

namespace Romanesco.Model.Services.Save
{
	public interface IProjectSaveService
	{
		public bool CanSave { get; }
		public bool CanExport { get; }
		Task SaveAsync();
		Task SaveAsAsync();
		Task ExportAsync();
	}
}
