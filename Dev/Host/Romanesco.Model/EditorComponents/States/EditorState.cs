using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Model.Commands;

namespace Romanesco.Model.EditorComponents.States
{
	internal abstract class EditorState : IEditorState
	{
		private readonly IModelFactory factory;
		protected IEditorStateChanger StateChanger { get; }

		public abstract string Title { get; }

		public abstract IProjectLoadService GetLoadService();
		public abstract IProjectSaveService GetSaveService();
		public abstract IProjectHistoryService GetHistoryService();

		protected EditorState(IModelFactory factory, IEditorStateChanger stateChanger)
		{
			this.factory = factory;
			this.StateChanger = stateChanger;
		}

		public virtual void OnCreate(IProjectContext project)
		{
			var projectFactory = factory.ResolveProjectModelFactory(project);
			StateChanger.ChangeState(projectFactory.ResolveNewEditorStateAsTransient());
		}

		public virtual void OnOpen(IProjectContext project)
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
		
		public void Undo(CommandAvailability availability)
		{
			GetHistoryService().Undo();
			OnUndo();
			availability.UpdateCanExecute(EditorCommandType.Undo, GetHistoryService().CanUndo);
		}
		
		public void Redo(CommandAvailability availability)
		{
			GetHistoryService().Redo();
			OnRedo();
			availability.UpdateCanExecute(EditorCommandType.Redo, GetHistoryService().CanRedo);
		}

		public void UpdateHistoryAvailability(CommandAvailability availability)
		{
			availability.UpdateCanExecute(GetHistoryService());
		}
		
		public void UpdateCanExecute(CommandAvailability availability)
		{
			availability.UpdateCanExecute(GetLoadService());
			availability.UpdateCanExecute(GetSaveService());
			availability.UpdateCanExecute(GetHistoryService());
		}
	}
}
