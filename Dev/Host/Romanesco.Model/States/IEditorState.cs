using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.States
{
	internal interface IEditorState
	{
		string Title { get; }

		IProjectLoadService GetLoadService();
		IProjectSaveService GetSaveService();
		IProjectHistoryService GetHistoryService();
	}
}
