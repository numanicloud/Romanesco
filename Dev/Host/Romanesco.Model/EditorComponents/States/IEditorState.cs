using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	internal interface IEditorState
	{
		string Title { get; }

		IProjectLoadService GetLoadService();
		IProjectSaveService GetSaveService();
		IProjectHistoryService GetHistoryService();

		void OnCreate(IProjectContext project);
		void OnOpen(IProjectContext project);
		void OnSave();
		void OnSaveAs();
		void OnEdit();
	}
}
