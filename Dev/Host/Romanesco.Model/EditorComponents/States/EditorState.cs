using System;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using System.Threading.Tasks;
using Romanesco.Model.Commands;
using static Romanesco.Model.EditorComponents.EditorCommandType;

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
		public virtual async Task<IProjectContext?> CreateAsync() => await GetLoadService().CreateAsync();

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
		
		public void Undo(IObserver<(EditorCommandType, bool)> observer, CommandAvailability availability)
		{
			var history = GetHistoryService();
			history.Undo();
			OnUndo();
			UpdateCanExecute(observer, EditorCommandType.Undo, history.CanUndo);
		}
		
		public void Redo(IObserver<(EditorCommandType, bool)> observer, CommandAvailability availability)
		{
			var history = GetHistoryService();
			history.Redo();
			OnRedo();
			UpdateCanExecute(observer, EditorCommandType.Redo, history.CanRedo);
		}

		public void UpdateHistoryAvailability(IObserver<(EditorCommandType, bool)> observer, CommandAvailability availability)
		{
			var history = GetHistoryService();
			UpdateCanExecute(observer, EditorCommandType.Undo, history.CanUndo);
			UpdateCanExecute(observer, EditorCommandType.Redo, history.CanRedo);
		}
		
		public void UpdateCanExecute(IObserver<(EditorCommandType, bool)> observer, CommandAvailability availability)
		{
			// IObserver<(EditorCommandType, bool)> をクラスに格上げしたいかも
			// 列挙子を使う代わりに対応した名前のメソッドを用意する
			// その中にLoadServiceなどを直接持たせて、CanCreateなどを読み取らせたい

			UpdateCanExecute(observer, Create, GetLoadService().CanCreate);
			UpdateCanExecute(observer, Open, GetLoadService().CanOpen);
			UpdateCanExecute(observer, Save, GetSaveService().CanSave);
			UpdateCanExecute(observer, SaveAs, GetSaveService().CanSave);
			UpdateCanExecute(observer, Export, GetSaveService().CanExport);
			UpdateCanExecute(observer, EditorCommandType.Undo, GetHistoryService().CanUndo);
			UpdateCanExecute(observer, EditorCommandType.Redo, GetHistoryService().CanRedo);
		}

		private static void UpdateCanExecute(IObserver<(EditorCommandType, bool)> observer, EditorCommandType commandType,
			bool canExecute)
		{
			observer.OnNext((commandType, canExecute));
		}
	}
}
