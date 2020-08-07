using System;
using System.Reactive.Subjects;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using static Romanesco.Model.EditorComponents.EditorCommandType;


namespace Romanesco.Model.Commands
{
	internal class CommandAvailability
	{
		private IObserver<(EditorCommandType, bool)> Observer { get; }

		public CommandAvailability(IObserver<(EditorCommandType, bool)> observer)
		{
			Observer = observer;
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
