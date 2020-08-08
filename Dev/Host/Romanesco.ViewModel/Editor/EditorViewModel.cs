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
            ReactiveCommand ToEditorCommand(EditorCommandType type)
            {
                var canExecute = editor.CanExecuteObservable
                    .Where(x => x.command == type)
                    .Select(x => x.canExecute)
                    .Concat(CommandExecution.IsUsing.Select(x => !x));
                return new ReactiveCommand(canExecute);
            }
            ReactiveCommand ToEditorCommand2(IObservable<bool> stream)
            {
	            var isNotUsing = CommandExecution.IsUsing.Select(x => !x);
	            return new ReactiveCommand(stream.Concat(isNotUsing).Do(x => {  }));
            }

            Editor = editor;
			this.interpreter = interpreter;
			CommandExecution = new BooleanUsingScopeSource();

            /* 各コマンドの実行可能性をUIに伝達する */
            //*
			var cav = Editor.CommandAvailability;
			CreateCommand = ToEditorCommand2(cav.CanCreate);
			OpenCommand = ToEditorCommand2(cav.CanOpen);
			SaveCommand = ToEditorCommand2(cav.CanSave);
			SaveAsCommand = ToEditorCommand2(cav.CanSaveAs);
			ExportCommand = ToEditorCommand2(cav.CanExport);
			Undo = ToEditorCommand2(cav.CanUndo);
			Redo = ToEditorCommand2(cav.CanRedo);
            //*/

            /* 各コマンドの実行内容を指定する */
            CreateCommand.SubscribeSafe(x => CreateAsync().Forget()).AddTo(editor.Disposables);
            OpenCommand.SubscribeSafe(x => OpenAsync().Forget()).AddTo(editor.Disposables);
            ExportCommand.SubscribeSafe(x => ExportAsync().Forget()).AddTo(editor.Disposables);
            SaveCommand.SubscribeSafe(x => SaveAsync().Forget()).AddTo(editor.Disposables);
            SaveAsCommand.SubscribeSafe(x => SaveAsAsync().Wait()).AddTo(editor.Disposables);
            Undo.SubscribeSafe(x => Editor.Undo()).AddTo(editor.Disposables);
            Redo.SubscribeSafe(x => Editor.Redo()).AddTo(editor.Disposables);

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
            using (CommandExecution.Create())
            {
                var projectContext = await Editor.CreateAsync();
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
                await Editor.ExportAsync();
            }
        }

        private async Task SaveAsync()
        {
            using (CommandExecution.Create())
            {
                await Editor.SaveAsync();
            }
        }

        private async Task SaveAsAsync()
        {
            using (CommandExecution.Create())
            {
                await Editor.SaveAsAsync();
            }
        }
    }
}
