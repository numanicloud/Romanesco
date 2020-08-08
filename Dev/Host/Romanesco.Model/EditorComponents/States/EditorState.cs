using System.Threading.Tasks;
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
		private readonly CommandAvailability commandAvailability;

		protected IEditorStateChanger StateChanger { get; }

		public abstract string Title { get; }

		public abstract IProjectLoadService GetLoadService();
		public abstract IProjectSaveService GetSaveService();
		public abstract IProjectHistoryService GetHistoryService();

		protected EditorState(IModelFactory factory, EditorSession editorSession)
		{
			// IEditorStateChanger, CommandAvailability はEditorの持っている物と同一である必要がある
			// それを名前で表現すべき。
			// EditorStateOwnership のような名前のクラスに、IEditorStateChangerとCommandAvailabilityを含ませるとよいかも
			this.factory = factory;
			this.StateChanger = editorSession.EditorStateChanger;
			this.commandAvailability = editorSession.CommandAvailability;
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

		/* 各コマンドを状態に応じて実行する */
		public virtual void OnEdit()
		{
		}

		public async Task<IProjectContext?> CreateAsync()
		{
			return await GetLoadService().CreateAsync();
		}

		public async Task<IProjectContext?> OpenAsync()
		{
			return await GetLoadService().OpenAsync();
		}
		
		public void NotifyEdit()
		{
			OnEdit();
			commandAvailability.UpdateCanExecute(GetHistoryService());
		}

		public async Task SaveAsync()
		{
			await GetSaveService().SaveAsync();
			OnSave();
		}
		
		public async Task SaveAsAsync()
		{
			await GetSaveService().SaveAsAsync();
			OnSaveAs();
		}
		
		public async Task ExportAsync()
		{
			await GetSaveService().ExportAsync();
		}
		
		public void Undo()
		{
			GetHistoryService().Undo();
			commandAvailability.UpdateCanExecute(EditorCommandType.Undo, GetHistoryService().CanUndo);
		}
		
		public void Redo()
		{
			GetHistoryService().Redo();
			commandAvailability.UpdateCanExecute(EditorCommandType.Redo, GetHistoryService().CanRedo);
		}

		public void UpdateCanExecute()
		{
			commandAvailability.UpdateCanExecute(GetLoadService());
			commandAvailability.UpdateCanExecute(GetSaveService());
			commandAvailability.UpdateCanExecute(GetHistoryService());
		}
	}
}
