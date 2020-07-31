using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	internal abstract class EditorState
	{
		private readonly IModelFactory factory;
		protected EditorStateChanger2 StateChanger { get; }

		public abstract string Title { get; }

		public abstract IProjectLoadService GetLoadService();
		public abstract IProjectSaveService GetSaveService();
		public abstract IProjectHistoryService GetHistoryService();

		protected EditorState(IModelFactory factory, EditorStateChanger2 stateChanger)
		{
			this.factory = factory;
			this.StateChanger = stateChanger;
		}

		public virtual void OnCreate(ProjectContext project)
		{
			var projectFactory = factory.ResolveProjectModelFactory(project);
			StateChanger.ChangeState(projectFactory.ResolveNewEditorStateAsTransient());
		}

		public virtual void OnOpen(ProjectContext project)
		{
			var projectFactory = factory.ResolveProjectModelFactory(project);
			StateChanger.ChangeState(projectFactory.ResolveCleanEditorStateAsTransient());
		}

		public virtual void OnSave()
		{
		}

		public virtual void OnSaveAs()
		{
		}

		public virtual void OnExport()
		{
		}

		public virtual void OnUndo()
		{
		}

		public virtual void OnRedo()
		{
		}

		public virtual void OnEdit()
		{
		}
	}
}
