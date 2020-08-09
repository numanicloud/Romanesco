using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
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
		private readonly IViewModelInterpreter interpreter;
		private readonly CommandManagerViewModel commandManager;

		public IEditorFacade Editor { get; set; }
		public ReactiveProperty<IStateViewModel[]> Roots { get; } = new ReactiveProperty<IStateViewModel[]>();

		public BooleanUsingScopeSource CommandExecution { get; }

		public ReactiveCommand CreateCommand => commandManager.Create;
		public ReactiveCommand OpenCommand => commandManager.Open;
		public ReactiveCommand SaveCommand => commandManager.Save;
		public ReactiveCommand SaveAsCommand { get; }
		public ReactiveCommand ExportCommand { get; }
		public ReactiveCommand Undo { get; }
		public ReactiveCommand Redo { get; }
		public ReactiveCommand GcDebugCommand { get; } = new ReactiveCommand();
		public List<IDisposable> Disposables => Editor.Disposables;

		public EditorViewModel(IEditorFacade editor, IViewModelInterpreter interpreter)
		{
			ReactiveCommand ToEditorCommand(IObservable<bool> stream)
			{
				var isNotUsing = CommandExecution.IsUsing.Select(x => !x);
				return new ReactiveCommand(stream.Concat(isNotUsing).Do(x => {  }), new SynchronizationContextScheduler(SynchronizationContext.Current));
			}

			Editor = editor;
			this.interpreter = interpreter;
			CommandExecution = new BooleanUsingScopeSource();

			commandManager = new CommandManagerViewModel(Editor.CommandAvailabilityPublisher, Roots, interpreter);

			/* 各コマンドの実行可能性をUIに伝達する */
			//*
			var cav = Editor.CommandAvailabilityPublisher;
			SaveAsCommand = ToEditorCommand(cav.CanSaveAs);
			ExportCommand = ToEditorCommand(cav.CanExport);
			Undo = ToEditorCommand(cav.CanUndo);
			Redo = ToEditorCommand(cav.CanRedo);
			//*/

			/* 各コマンドの実行内容を指定する */
			ExportCommand.SubscribeSafe(x => ExportAsync().Forget())
				.AddTo(editor.Disposables);

			SaveAsCommand.SubscribeSafe(x => SaveAsAsync().Wait())
				.AddTo(editor.Disposables);

			Undo.SubscribeSafe(x => Editor.CommandAvailabilityPublisher.Undo())
				.AddTo(editor.Disposables);

			Redo.SubscribeSafe(x => Editor.CommandAvailabilityPublisher.Redo())
				.AddTo(editor.Disposables);

			GcDebugCommand.SubscribeSafe(x => GC.Collect()).AddTo(editor.Disposables);

			//Messenger.Raise(new TransitionMessage())
		}

		public void ShowProjectSetting(ProjectSettingsEditor editor)
		{
			var vm = new ProjectSettingsEditorViewModel(editor);
			Messenger.Raise(new TransitionMessage(vm, "CreateProject"));
		}

		private async Task ExportAsync()
		{
			using (CommandExecution.Create())
			{
				await Editor.CommandAvailabilityPublisher.ExportAsync();
			}
		}

		private async Task SaveAsAsync()
		{
			using (CommandExecution.Create())
			{
				await Editor.CommandAvailabilityPublisher.SaveAsAsync();
			}
		}
	}
}
