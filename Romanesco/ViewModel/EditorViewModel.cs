using Reactive.Bindings;
using Romanesco.Annotations;
using Romanesco.Common.Utility;
using Romanesco.Extensibility;
using System;
using System.Linq;
using System.Windows.Controls;

namespace Romanesco.View
{
    class EditorViewModel
    {
        public ProjectContext ProjectContext { get; set; }
        public StateViewContext Root { get; set; }

        public ReactiveCommand CreateCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand ExportCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand OpenCommand { get; } = new ReactiveCommand();
        public ReactiveCommand SaveCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand Undo { get; } = new ReactiveCommand();
        public ReactiveCommand Redo { get; } = new ReactiveCommand();

        public void Initialize()
        {
            ProjectContext = new ProjectContext();

            var loader = new PluginLoader(ProjectContext);
            var extensions = loader.Load("Plugins");
            var stateInterpreter = new Model.ObjectInterpreter(extensions.StateFactories);
            var viewModelInterpreter = new ViewModel.ViewModelInterpreter(extensions.StateViewModelFactories);
            var viewInterpreter = new ViewInterpreter(extensions.ViewFactories);

            var rootProperty = typeof(Sample.Project).GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(EditorMemberAttribute), true).Length == 1)
                .First();

            var state = stateInterpreter.InterpretAsState(rootProperty);
            var viewModel = viewModelInterpreter.InterpretAsViewModel(state);
            var view = viewInterpreter.InterpretAsView(viewModel);
            view.OnError.Subscribe(ex =>
            {
                throw ex;
            });

            Root = view;

            Undo.Subscribe(() => ProjectContext.CommandHistory.Undo());
            Redo.Subscribe(() => ProjectContext.CommandHistory.Redo());
        }
    }
}
