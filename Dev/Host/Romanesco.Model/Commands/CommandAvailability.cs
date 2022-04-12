using System;
using System.Reactive;
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
	 * 次のステップへ行く前に、恐らくステート切り替えが起きると
	 * CommandAvailability が同一であると思って書いている部分がバグると思われるので整理したい
	 *
	 * まずは CommandAvailability あるいはクッションとなるクラスに、
	 * ステートが切り替わった際のインパクトを吸収させたい
	 */

	internal class CommandAvailability : IDisposable, ICommandAvailabilityPublisher
	{
		private readonly ReplaySubject<(EditorCommandType command, bool canExecute)> _canExecuteSubject = new();
		private readonly IEditorState _currentState;
		private readonly CreateCommand _createCommand;
		private readonly OpenCommand _openCommand;
		private readonly SaveCommand _saveCommand;
		private readonly SaveAsCommand _saveAsCommand;
		private readonly ExportCommand _exportCommand;
		private readonly UndoCommand _undoCommand;
		private readonly RedoCommand _redoCommand;

		private IObserver<(EditorCommandType, bool)> Observer => _canExecuteSubject;

		public IObservable<(EditorCommandType command, bool canExecute)> Observable => _canExecuteSubject;

		/* 各コマンドの実行可能性を保持する */
		public IReadOnlyReactiveProperty<bool> CanSave => _saveCommand.CanExecute;
		public IReadOnlyReactiveProperty<bool> CanSaveAs => _saveAsCommand.CanExecute;
		public IReadOnlyReactiveProperty<bool> CanExport => _exportCommand.CanExecute;
		public IReadOnlyReactiveProperty<bool> CanCreate => _createCommand.CanExecute;
		public IReadOnlyReactiveProperty<bool> CanOpen => _openCommand.CanExecute;
		public IReadOnlyReactiveProperty<bool> CanUndo => _undoCommand.CanExecute;
		public IReadOnlyReactiveProperty<bool> CanRedo => _redoCommand.CanExecute;

		public IObservable<IProjectContext> OnCreate => _createCommand.OnExecuted;
		public IObservable<IProjectContext> OnOpen => _openCommand.OnExecuted;
		public IObservable<Unit> OnSaveAs => _saveAsCommand.OnExecuted;
		
		public CommandAvailability(IEditorState currentState, IEditorStateRepository repository)
		{
			IObservable<bool> GetCanExecute(EditorCommandType type)
			{
				return _canExecuteSubject.Where(x => x.command == type)
					.Select(x => x.canExecute);
			}

			_createCommand = new CreateCommand(GetCanExecute(Create), currentState);
			_openCommand = new OpenCommand(GetCanExecute(Open), currentState);
			_saveCommand = new SaveCommand(GetCanExecute(Save), currentState);
			_saveAsCommand = new SaveAsCommand(GetCanExecute(SaveAs), currentState);
			_exportCommand = new ExportCommand(GetCanExecute(Export), currentState);
			_undoCommand = new UndoCommand(GetCanExecute(EditorCommandType.Undo), currentState);
			_redoCommand = new RedoCommand(GetCanExecute(EditorCommandType.Redo), currentState);

			_undoCommand.OnExecuted.Subscribe(x => UpdateCanExecute(EditorCommandType.Undo, currentState.GetHistoryService().CanUndo));
			_redoCommand.OnExecuted.Subscribe(x => UpdateCanExecute(EditorCommandType.Redo, currentState.GetHistoryService().CanRedo));

			this._currentState = currentState;
		}

		public void Dispose()
		{
			_canExecuteSubject.Dispose();
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
			UpdateCanExecute(EditorCommandType.Undo, history.CanUndo);
			UpdateCanExecute(EditorCommandType.Redo, history.CanRedo);
		}

		// ここで全ての実行可能性を一律で更新していると、
		// Load,Save,History が常に揃っている必要がありテストがしづらい
		// 各コマンドをモックして、LoadServiceなどが要らないバージョンを作れるようにすべき？
		// コマンド自身が CommandRouter のようにステートを参照すべき？
		public void UpdateCanExecute()
		{
			UpdateCanExecute(_currentState.GetLoadService());
			UpdateCanExecute(_currentState.GetSaveService());
			UpdateCanExecute(_currentState.GetHistoryService());
		}

		/* コマンドを実行する */
		public async Task<IProjectContext?> CreateAsync() => await _createCommand.Execute();

		public async Task<IProjectContext?> OpenAsync() => await _openCommand.Execute();

		public async Task SaveAsync() => await _saveCommand.Execute();

		public async Task SaveAsAsync() => await _saveAsCommand.Execute();

		public async Task ExportAsync() => await _exportCommand.Execute();

		public void Undo() => _undoCommand.Execute();

		public void Redo() => _redoCommand.Execute();

		public void NotifyEdit()
		{
			_currentState.OnEdit();
			UpdateCanExecute(_currentState.GetHistoryService());
		}
	}
}
