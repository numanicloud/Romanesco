using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.States
{
	internal class EmptyEditorState : EditorState
	{
		private readonly IProjectLoadService _loadService;

		public override string Title => "Romanesco - プロジェクトなし";

		public EmptyEditorState(IProjectLoadService loadService)
		{
			this._loadService = loadService;
		}

		public override IProjectLoadService GetLoadService() => _loadService;

		public override IProjectSaveService GetSaveService() => new NullSaveService();

		public override IProjectHistoryService GetHistoryService() => new NullHistoryService();
	}
}
