using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Common.Interfaces;
using Romanesco.Common.Utility;
using Romanesco.Model;
using Romanesco.ViewModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System;

namespace Romanesco.View
{
    class EditorViewModel
    {
        private Task runningCommandTask = null;

        public Editor Editor { get; set; }
        public ReactiveProperty<IStateViewModel[]> Roots { get; } = new ReactiveProperty<IStateViewModel[]>();

        public ReactiveCommand CreateCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand ExportCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand OpenCommand { get; } = new ReactiveCommand();
        public ReactiveCommand SaveCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand SaveAsCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand Undo { get; } = new ReactiveCommand();
        public ReactiveCommand Redo { get; } = new ReactiveCommand();
        public ReactiveCommand GcDebugCommand { get; } = new ReactiveCommand();

        public EditorViewModel(Editor editor, IStateViewModelFactoryProvider factoryProvider)
        {
            ViewModelInterpreter CreateInterpreter()
            {
                return new ViewModelInterpreter(factoryProvider.GetStateViewModelFactories().ToArray());
            }

            Editor = editor;

            CreateCommand.Where(x => runningCommandTask == null)
                .SubscribeSafe(x =>
            {
                var interpreter = CreateInterpreter();
                var project = Editor.Create();
                Roots.Value = project.Root.States.Select(s => interpreter.InterpretAsViewModel(s)).ToArray();
            });

            OpenCommand.Where(x => runningCommandTask == null)
                .SubscribeSafe(x => Open(factoryProvider));

            ExportCommand.SubscribeSafe(x => Editor.Export());
            SaveCommand.SubscribeSafe(x => Editor.SaveAsync().Wait());
            SaveAsCommand.SubscribeSafe(x => Editor.SaveAsAsync().Wait());

            Undo.SubscribeSafe(x => Editor.Undo());
            Redo.SubscribeSafe(x => Editor.Redo());

            GcDebugCommand.SubscribeSafe(x => GC.Collect());
        }

        private async Task Open(IStateViewModelFactoryProvider factoryProvider)
        {
            var interpreter = new ViewModelInterpreter(factoryProvider.GetStateViewModelFactories().ToArray());
            var project = await Editor.OpenAsync();
            Roots.Value = project.Root.States.Select(s => interpreter.InterpretAsViewModel(s)).ToArray();
            runningCommandTask = null;
        }
    }
}
