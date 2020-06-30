using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	class CleanEditorState : EditorState
	{
		private readonly IProjectLoadService loadService;
		private readonly IProjectSaveService saveService;
		private readonly IProjectHistoryService historyService;
		private readonly IServiceLocator serviceLocator;

		public override string Title => $"Romanesco - {System.IO.Path.GetFileName(Context.CurrentProject?.Project.DefaultSavePath ?? "")}";

		public EditorContext Context { get; }

		public CleanEditorState(IProjectLoadService loadService,
			IProjectHistoryService historyService,
			IServiceLocator serviceLocator,
			EditorContext context)
		{
			Context = context;
			this.loadService = loadService;
			this.saveService = serviceLocator.CreateInstance<IProjectSaveService>(context.CurrentProject);
			this.historyService = historyService;
			this.serviceLocator = serviceLocator;
		}

		public override IProjectLoadService GetLoadService() => loadService;

		public override IProjectSaveService GetSaveService() => saveService;

		public override IProjectHistoryService GetHistoryService() => historyService;

		public override void OnEdit()
		{
			Context.Editor.ChangeState(serviceLocator.CreateInstance<DirtyEditorState>(Context));
		}
	}
}
