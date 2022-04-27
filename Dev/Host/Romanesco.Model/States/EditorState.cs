using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.States
{
	internal abstract class EditorState : IEditorState
	{
		public abstract string Title { get; }

		public abstract IProjectLoadService GetLoadService();
		public abstract IProjectSaveService GetSaveService();
		public abstract IProjectHistoryService GetHistoryService();
	}
}
