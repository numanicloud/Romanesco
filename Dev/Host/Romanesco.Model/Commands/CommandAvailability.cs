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
	internal class CommandAvailability : IDisposable, ICommandAvailabilityPublisher
	{
		private readonly ReplaySubject<(EditorCommandType command, bool canExecute)> canExecuteSubject
			= new ReplaySubject<(EditorCommandType command, bool canExecute)>();
		private readonly IEditorState currentState;
		private readonly CreateCommand _createCommand;
		private readonly OpenCommand _openCommand;

		private readonly Subject<Unit> onSaveAsSubject = new Subject<Unit>();

		private IObserver<(EditorCommandType, bool)> Observer => canExecuteSubject;

		public IObservable<(EditorCommandType command, bool canExecute)> Observable => canExecuteSubject;

		/* 各コマンドの実行可能性を保持する */
		public IReadOnlyReactiveProperty<bool> CanSave { get; }
		public IReadOnlyReactiveProperty<bool> CanSaveAs { get; }
		public IReadOnlyReactiveProperty<bool> CanExport { get; }
		public IReadOnlyReactiveProperty<bool> CanCreate => _createCommand.CanExecute;
		public IReadOnlyReactiveProperty<bool> CanOpen => _openCommand.CanExecute;
		public IReadOnlyReactiveProperty<bool> CanUndo { get; }
		public IReadOnlyReactiveProperty<bool> CanRedo { get; }

		public IObservable<IProjectContext> OnCreate => _createCommand.OnExecuted;
		public IObservable<IProjectContext> OnOpen => _openCommand.OnExecuted;
		public IObservable<Unit> OnSaveAs => onSaveAsSubject;


		public CommandAvailability(IEditorState currentState)
		{
			IReadOnlyReactiveProperty<bool> MakeProperty(EditorCommandType type)
			{
				var stream = canExecuteSubject.Where(x => x.command == type)
					.Select(x => x.canExecute);
				return new ReactiveProperty<bool>(stream);
			}

			IObservable<bool> GetCanExecute(EditorCommandType type)
			{
				return canExecuteSubject.Where(x => x.command == type)
					.Select(x => x.canExecute);
			}

			_createCommand = new CreateCommand(GetCanExecute(Create), currentState);
			_openCommand = new OpenCommand(GetCanExecute(Open), currentState);

			CanSave = MakeProperty(Save);
			CanSaveAs = MakeProperty(SaveAs);
			CanExport = MakeProperty(Export);
			CanUndo = MakeProperty(EditorCommandType.Undo);
			CanRedo = MakeProperty(EditorCommandType.Redo);
			this.currentState = currentState;
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
			UpdateCanExecute(EditorCommandType.Undo, history.CanUndo);
			UpdateCanExecute(EditorCommandType.Redo, history.CanRedo);
		}

		public void UpdateCanExecute()
		{
			UpdateCanExecute(currentState.GetLoadService());
			UpdateCanExecute(currentState.GetSaveService());
			UpdateCanExecute(currentState.GetHistoryService());
		}

		/* コマンドを実行する */
		public async Task<IProjectContext?> CreateAsync() => await _createCommand.Execute();

		public async Task<IProjectContext?> OpenAsync() => await _openCommand.Execute();

		public async Task SaveAsync()
		{
			await currentState.GetSaveService().SaveAsync();
			currentState.OnSave();
		}

		public async Task SaveAsAsync()
		{
			await currentState.GetSaveService().SaveAsAsync();
			currentState.OnSaveAs();
			onSaveAsSubject.OnNext(Unit.Default);
		}

		public async Task ExportAsync()
		{
			await currentState.GetSaveService().ExportAsync();
		}

		public void Undo()
		{
			currentState.GetHistoryService().Undo();
			UpdateCanExecute(EditorCommandType.Undo, currentState.GetHistoryService().CanUndo);
		}

		public void Redo()
		{
			currentState.GetHistoryService().Redo();
			UpdateCanExecute(EditorCommandType.Redo, currentState.GetHistoryService().CanRedo);
		}

		public void NotifyEdit()
		{
			currentState.OnEdit();
			UpdateCanExecute(currentState.GetHistoryService());
		}
	}
}
