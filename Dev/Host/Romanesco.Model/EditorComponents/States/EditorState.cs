using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	internal abstract class EditorState
	{
		protected EditorStateChanger StateChanger { get; }

		public abstract string Title { get; }

		public abstract IProjectLoadService GetLoadService();
		public abstract IProjectSaveService GetSaveService();
		public abstract IProjectHistoryService GetHistoryService();

		public EditorState(EditorStateChanger stateChanger)
		{
			this.StateChanger = stateChanger;
		}

		public virtual void OnCreate(ProjectContext project)
		{
			StateChanger.ChangeToNew(project);
		}

		public virtual void OnOpen(ProjectContext project)
		{
			StateChanger.ChangeToClean(project);
		}

		public virtual void OnSave()
		{
			StateChanger.ChangeToClean();
		}

		public virtual void OnSaveAs()
		{
			StateChanger.ChangeToClean();
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
			StateChanger.ChangeToDirty();
		}
	}
}
