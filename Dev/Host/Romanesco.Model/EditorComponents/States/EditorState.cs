using System;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using System.Threading.Tasks;

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

		// 怠けでvirtualにしているが、このクラスに更に基底の IEditorState とかが必要かもしれない
		public virtual async Task<ProjectContext?> CreateAsync() => await GetLoadService().CreateAsync();

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
		
		public void UpdateCanExecute(IObserver<(EditorCommandType, bool)> observer)
		{
			// IObserver<(EditorCommandType, bool)> をクラスに格上げしたいかも
			// 列挙子を使う代わりに対応した名前のメソッドを用意する
			// その中にLoadServiceなどを直接持たせて、CanCreateなどを読み取らせたい

			observer.OnNext((EditorCommandType.Create, GetLoadService().CanCreate));
			observer.OnNext((EditorCommandType.Open, GetLoadService().CanOpen));
			observer.OnNext((EditorCommandType.Save, GetSaveService().CanSave));
			observer.OnNext((EditorCommandType.SaveAs, GetSaveService().CanSave));
			observer.OnNext((EditorCommandType.Export, GetSaveService().CanExport));
			observer.OnNext((EditorCommandType.Undo, GetHistoryService().CanUndo));
			observer.OnNext((EditorCommandType.Redo, GetHistoryService().CanRedo));
		}
	}
}
