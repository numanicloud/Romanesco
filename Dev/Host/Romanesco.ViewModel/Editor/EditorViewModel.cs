using System;
using System.Collections.Generic;
using System.Windows.Input;
using Livet.Messaging;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.ViewModel.Commands;
using Romanesco.ViewModel.Project;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Editor
{
	internal class EditorViewModel : Livet.ViewModel, IEditorViewModel
	{
		private readonly CommandManagerViewModel commandManager;

		public ReactiveProperty<IStateViewModel[]> Roots { get; } = new ReactiveProperty<IStateViewModel[]>();

		public ICommand CreateCommand => commandManager.Create;
		public ICommand OpenCommand => commandManager.Open;
		public ICommand SaveCommand => commandManager.Save;
		public ICommand SaveAsCommand => commandManager.SaveAs;
		public ICommand ExportCommand => commandManager.Export;
		public ICommand Undo => commandManager.Undo;
		public ReactiveCommand Redo => commandManager.Redo;
		public ReactiveCommand GcDebugCommand { get; } = new ReactiveCommand();
		public List<IDisposable> Disposables { get; }

		public EditorViewModel(IEditorFacade editor, IViewModelInterpreter interpreter)
		{
			Disposables = editor.Disposables;
			commandManager = new CommandManagerViewModel(editor.CommandAvailabilityPublisher, Roots, interpreter);
			GcDebugCommand.SubscribeSafe(x => GC.Collect()).AddTo(editor.Disposables);
		}

		public void ShowProjectSetting(ProjectSettingsEditor editor)
		{
			var vm = new ProjectSettingsEditorViewModel(editor);
			Messenger.Raise(new TransitionMessage(vm, "CreateProject"));
		}
	}
}
