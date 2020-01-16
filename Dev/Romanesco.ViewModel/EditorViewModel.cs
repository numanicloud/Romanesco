using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Romanesco.Common.Model;
using Romanesco.Model.EditorComponents;
using Romanesco.ViewModel.States;
using Livet.Messaging;
using Romanesco.Common.Model.Helpers;

namespace Romanesco.ViewModel
{
    public class EditorViewModel : Livet.ViewModel
    {
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

        public EditorViewModel(IEditorFacade editor, IStateViewModelFactoryProvider factoryProvider)
        {
            ReactiveCommand ToEditorCommand(EditorCommandType type)
            {
                var canExecute = editor.CanExecuteObservable
                    .Where(x => x.command == type)
                    .Select(x => x.canExecute)
                    .Concat(CommandExecution.IsUsing.Select(x => !x));
                return new ReactiveCommand(canExecute);
            }

            Editor = editor;
            CommandExecution = new BooleanUsingScopeSource();

            CreateCommand = ToEditorCommand(EditorCommandType.Create);
            OpenCommand = ToEditorCommand(EditorCommandType.Open);
            SaveCommand = ToEditorCommand(EditorCommandType.Save);
            SaveAsCommand = ToEditorCommand(EditorCommandType.SaveAs);
            ExportCommand = ToEditorCommand(EditorCommandType.Export);
            Undo = ToEditorCommand(EditorCommandType.Undo);
            Redo = ToEditorCommand(EditorCommandType.Redo);

            CreateCommand.SubscribeSafe(x => Create(factoryProvider));
            OpenCommand.SubscribeSafe(x => OpenAsync(factoryProvider).Forget());
            ExportCommand.SubscribeSafe(x => ExportAsync().Forget());
            SaveCommand.SubscribeSafe(x => SaveAsync().Forget());
            SaveAsCommand.SubscribeSafe(x => SaveAsAsync().Wait());
            Undo.SubscribeSafe(x => Editor.Undo());
            Redo.SubscribeSafe(x => Editor.Redo());

            GcDebugCommand.SubscribeSafe(x => GC.Collect());

            //Messenger.Raise(new TransitionMessage())
        }

        public void ShowProjectSetting(ProjectSettingsEditor editor)
        {
            var vm = new ProjectSettingsEditorViewModel(editor);
            Messenger.Raise(new TransitionMessage(vm, "CreateProject"));
        }

        private ViewModelInterpreter CreateInterpreter(IStateViewModelFactoryProvider factoryProvider)
        {
            return new ViewModelInterpreter(factoryProvider.GetStateViewModelFactories().ToArray());
        }

        private void Create(IStateViewModelFactoryProvider factoryProvider)
        {
            using (CommandExecution.Create())
            {
                var interpreter = CreateInterpreter(factoryProvider);
                var projectContext = Editor.Create();
                if (projectContext != null)
                {
                    Roots.Value = projectContext.Project.Root.States
                        .Select(s => interpreter.InterpretAsViewModel(s))
                        .ToArray();
                }
            }
        }

        private async Task OpenAsync(IStateViewModelFactoryProvider factoryProvider)
        {
            using (CommandExecution.Create())
            {
                var interpreter = CreateInterpreter(factoryProvider);
                var projectContext = await Editor.OpenAsync();
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
