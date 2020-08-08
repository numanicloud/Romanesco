using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.Model.Commands
{
	internal class CommandAvailability : IDisposable
	{
		private readonly ReplaySubject<(EditorCommandType command, bool canExecute)> canExecuteSubject
			= new ReplaySubject<(EditorCommandType command, bool canExecute)>();

		private IObserver<(EditorCommandType, bool)> Observer => canExecuteSubject;

		public IObservable<(EditorCommandType command, bool canExecute)> Observable => canExecuteSubject;

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
	}
}
