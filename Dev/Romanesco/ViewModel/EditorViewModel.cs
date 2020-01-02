using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model;
using Romanesco.ViewModel;
using System.Linq;

namespace Romanesco.View
{
    class EditorViewModel
    {
        public Editor Editor { get; set; }

        public ReactiveCommand CreateCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand ExportCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand OpenCommand { get; } = new ReactiveCommand();
        public ReactiveCommand SaveCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand SaveAsCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand Undo { get; } = new ReactiveCommand();
        public ReactiveCommand Redo { get; } = new ReactiveCommand();

        public EditorViewModel(Editor editor)
        {
            Editor = editor;

            CreateCommand.Subscribe(() => Editor.Create());
            ExportCommand.Subscribe(() => Editor.Export());
            OpenCommand.Subscribe(() => Editor.Open());
            SaveCommand.Subscribe(() => Editor.Save());
            SaveAsCommand.Subscribe(() => Editor.SaveAs());
            Undo.Subscribe(() => Editor.Undo());
            Redo.Subscribe(() => Editor.Redo());
        }

        public IStateViewModel[] LoadRoots(IStateViewModelFactory[] factories, IFieldState[] roots)
        {
            var interpreter = new ViewModelInterpreter(factories);
            return roots.Select(s => interpreter.InterpretAsViewModel(s)).ToArray();
        }
    }
}
