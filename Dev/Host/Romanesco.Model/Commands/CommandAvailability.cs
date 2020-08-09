using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Reactive.Bindings;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Interfaces;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.Model.Commands
{
	/*
	 * コマンド実装方針
	 * Model側のコマンドクラス、ViewModel側のコマンドクラスで分ける。(CommandAvailabilityViewModelとする)
	 * コマンドの実行をEditorStateから剥がし、まずはここに持ってくる
	 * ReactiveCommandをSubscribeする機能をこのクラスに持ってくる(あるいはViewModel側に)
	 * ViewModel側から、Editor.CommandAvailability.Create というようにコマンドに直接アクセスする
	 * クライアントクラスは EditorViewModel でもいいが、最終的には CommandAvailabilityViewModel が理想かな？
	 */

	internal class CommandAvailability : IDisposable, ICommandAvailabilityPublisher
	{
		private readonly ReplaySubject<(EditorCommandType command, bool canExecute)> canExecuteSubject
			= new ReplaySubject<(EditorCommandType command, bool canExecute)>();

		private IObserver<(EditorCommandType, bool)> Observer => canExecuteSubject;

		public IObservable<(EditorCommandType command, bool canExecute)> Observable => canExecuteSubject;

		/* 各コマンドの実行可能性を保持する */
		public IReadOnlyReactiveProperty<bool> CanSave { get; }
		public IReadOnlyReactiveProperty<bool> CanSaveAs { get; }
		public IReadOnlyReactiveProperty<bool> CanExport { get; }
		public IReadOnlyReactiveProperty<bool> CanCreate { get; }
		public IReadOnlyReactiveProperty<bool> CanOpen { get; }
		public IReadOnlyReactiveProperty<bool> CanUndo { get; }
		public IReadOnlyReactiveProperty<bool> CanRedo { get; }

		public CommandAvailability()
		{
			IReadOnlyReactiveProperty<bool> MakeProperty(EditorCommandType type)
			{
				var stream = canExecuteSubject.Where(x => x.command == type)
					.Select(x => x.canExecute);
				return new ReactiveProperty<bool>(stream);
			}

			CanSave = MakeProperty(Save);
			CanSaveAs = MakeProperty(SaveAs);
			CanExport = MakeProperty(Export);
			CanCreate = MakeProperty(Create);
			CanOpen = MakeProperty(Open);
			CanUndo = MakeProperty(Undo);
			CanRedo = MakeProperty(Redo);
		}

		public void Dispose()
		{
			canExecuteSubject.Dispose();
		}

		public void UpdateCanExecute(EditorCommandType commandType, bool canExecute)
		{
			Observer.OnNext((commandType, canExecute));
		}

		public void UpdateCanExecute(IProjectSaveService save)
		{
			UpdateCanExecute(Save, save.CanSave);
			UpdateCanExecute(SaveAs, save.CanSave);
			UpdateCanExecute(Export, save.CanExport);
		}

		public void UpdateCanExecute(IProjectLoadService load)
		{
			UpdateCanExecute(Create, load.CanCreate);
			UpdateCanExecute(Open, load.CanOpen);
		}

		public void UpdateCanExecute(IProjectHistoryService history)
		{
			UpdateCanExecute(Undo, history.CanUndo);
			UpdateCanExecute(Redo, history.CanRedo);
		}
		
		public async Task<IProjectContext?> CreateAsync(IEditorState editorState)
		{
			return await editorState.GetLoadService().CreateAsync();
		}

		public async Task<IProjectContext?> OpenAsync(IEditorState editorState)
		{
			return await editorState.GetLoadService().OpenAsync();
		}

		public async Task SaveAsync(IEditorState editorState)
		{
			await editorState.GetSaveService().SaveAsync();
			editorState.OnSave();
		}
		
		public async Task SaveAsAsync(IEditorState editorState)
		{
			await editorState.GetSaveService().SaveAsAsync();
			editorState.OnSaveAs();
		}
		
		public async Task ExportAsync(IEditorState editorState)
		{
			await editorState.GetSaveService().ExportAsync();
		}
	}
}
