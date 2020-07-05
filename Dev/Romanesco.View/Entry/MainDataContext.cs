using System;
using System.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.View.States;
using Romanesco.ViewModel.Editor;

namespace Romanesco.View.Entry
{
	internal class MainDataContext : IEditorViewContext
    {
        private ReactiveProperty<StateRootDataContext?> rootProperty = new ReactiveProperty<StateRootDataContext?>();
        private IDisposable? errorSubscription;
		private readonly ViewInterpreter interpreter;

		public IReadOnlyReactiveProperty<StateRootDataContext?> Root => rootProperty;
        public IEditorViewModel Editor { get; }

        public MainDataContext(IEditorViewModel editor, ViewInterpreter interpreter)
        {
            Editor = editor;
            editor.Roots.Subscribe(roots => LoadRoot(roots))
	            .AddTo(editor.Disposables);
			this.interpreter = interpreter;

            editor.Disposables.Add(rootProperty);
		}

        private void LoadRoot(IStateViewModel[] vmRoots)
        {
            if (vmRoots == null)
            {
                rootProperty.Value = null;
                return;
            }

            var views = vmRoots.Select(vm => interpreter.InterpretAsView(vm)).ToArray();
            rootProperty.Value = new StateRootDataContext(views);

            errorSubscription?.Dispose();
            errorSubscription = rootProperty.Value.OnError.Subscribe(x =>
            {
                throw x;
            }).AddTo(Editor.Disposables);
        }
    }
}
