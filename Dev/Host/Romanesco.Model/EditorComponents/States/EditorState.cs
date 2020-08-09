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

		public void NotifyEdit()
		{
			commandAvailability.NotifyEdit(this);
		}
	}
}
