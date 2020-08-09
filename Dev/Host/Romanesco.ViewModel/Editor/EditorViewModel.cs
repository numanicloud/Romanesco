using System;
using System.Collections.Generic;
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

		public ReactiveCommand CreateCommand => commandManager.Create;
		public ReactiveCommand OpenCommand => commandManager.Open;
		public ReactiveCommand SaveCommand => commandManager.Save;
		public ReactiveCommand SaveAsCommand => commandManager.SaveAs;
		public ReactiveCommand ExportCommand => commandManager.Export;
		public ReactiveCommand Undo => commandManager.Undo;
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
