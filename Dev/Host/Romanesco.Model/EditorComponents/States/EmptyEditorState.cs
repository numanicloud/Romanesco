using Romanesco.Model.Commands;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	internal class EmptyEditorState : EditorState
	{
		private readonly IProjectLoadService loadService;

		public override string Title => "Romanesco - プロジェクトなし";

		public EmptyEditorState(IProjectLoadService loadService,
			IModelFactory factory,
			EditorSession editorSession)
			: base(factory, editorSession)
		{
			this.loadService = loadService;
		}

		public override IProjectLoadService GetLoadService() => loadService;

		public override IProjectSaveService GetSaveService() => new NullSaveService();

		public override IProjectHistoryService GetHistoryService() => new NullHistoryService();
	}
}
