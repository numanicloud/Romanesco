using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Livet.Messaging;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.ViewModel.Project;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Editor
{
	internal class EditorViewModel : Livet.ViewModel, IEditorViewModel
    {
		private readonly IViewModelInterpreter interpreter;

		public IEditorFacade Editor { get; set; }
        public ReactiveProperty<IStateViewModel[]> Roots { get; } = new ReactiveProperty<IStateViewModel[]>();

        public BooleanUsingScopeSource CommandExecution { get; }

        public ReactiveCommand CreateCommand { get; set; }
        public ReactiveCommand OpenCommand { get; }
        public ReactiveCommand SaveCommand { get; set; }
        public ReactiveCommand SaveAsCommand { get; set; }
        public ReactiveCommand ExportCommand { get; set; }
        public ReactiveCommand Undo { get; }
        public ReactiveCommand Redo { get; }
        public ReactiveCommand GcDebugCommand { get; } = new ReactiveCommand();
        public List<IDisposable> Disposables => Editor.Disposables;

        public EditorViewModel(IEditorFacade editor, IViewModelInterpreter interpreter)
        {
			ReactiveCommand ToEditorCommand(IObservable<bool> stream)
            {
	            var isNotUsing = CommandExecution.IsUsing.Select(x => !x);
	            return new ReactiveCommand(stream.Concat(isNotUsing).Do(x => {  }));
            }

            Editor = editor;
			this.interpreter = interpreter;
			CommandExecution = new BooleanUsingScopeSource();

            /* 各コマンドの実行可能性をUIに伝達する */
            //*
			var cav = Editor.CommandAvailabilityPublisher;
			CreateCommand = ToEditorCommand(cav.CanCreate);
			OpenCommand = ToEditorCommand(cav.CanOpen);
			SaveCommand = ToEditorCommand(cav.CanSave);
			SaveAsCommand = ToEditorCommand(cav.CanSaveAs);
			ExportCommand = ToEditorCommand(cav.CanExport);
			Undo = ToEditorCommand(cav.CanUndo);
			Redo = ToEditorCommand(cav.CanRedo);
            //*/

            /* 各コマンドの実行内容を指定する */
            CreateCommand.SubscribeSafe(x => CreateAsync().Forget())
	            .AddTo(editor.Disposables);

            OpenCommand.SubscribeSafe(x => OpenAsync().Forget())
	            .AddTo(editor.Disposables);

            ExportCommand.SubscribeSafe(x => ExportAsync().Forget())
	            .AddTo(editor.Disposables);

            SaveCommand.SubscribeSafe(x => SaveAsync().Forget())
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

        private async Task CreateAsync()
        {
            // 実行をロックする機能は IProjectLoadService 側に入れるべきかも
            using (CommandExecution.Create())
            {
                var projectContext = await Editor.CreateAsync();
                // Editor 側のメソッドに副作用があるので、インライン化は保留
                if (projectContext != null)
                {
                    Roots.Value = projectContext.Project.Root.States
                        .Select(s => interpreter.InterpretAsViewModel(s))
                        .ToArray();
                }
            }
        }

        private async Task OpenAsync()
        {
            using (CommandExecution.Create())
            {
                var projectContext = await Editor.OpenAsync();
                // Editor 側のメソッドに副作用があるので、インライン化は保留
                if (projectContext == null)
                {
                    return;
                }

                Roots.Value = projectContext.Project.Root.States
                    .Select(s => interpreter.InterpretAsViewModel(s))
                    .ToArray();
            }
        }

        private async Task ExportAsync()
        {
            using (CommandExecution.Create())
            {
                await Editor.CommandAvailabilityPublisher.ExportAsync();
            }
        }

        private async Task SaveAsync()
        {
            using (CommandExecution.Create())
            {
                await Editor.CommandAvailabilityPublisher.SaveAsync();
            }
        }

        private async Task SaveAsAsync()
        {
            using (CommandExecution.Create())
            {
                await Editor.SaveAsAsync();
                // Editor 側のメソッドに副作用があるので、インライン化は保留
            }
        }
    }
}
