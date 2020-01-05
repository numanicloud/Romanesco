using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romanesco.View
{
    class MainDataContext
    {
        private ReactiveProperty<StateRootDataContext> rootProperty = new ReactiveProperty<StateRootDataContext>();
        private IDisposable errorSubscription;
        private readonly IViewFactoryProvider factoryProvider;

        public EditorViewModel Editor { get; }
        public IReadOnlyReactiveProperty<StateRootDataContext> Root => rootProperty;

        public MainDataContext(EditorViewModel editor, IViewFactoryProvider factoryProvider)
        {
            Editor = editor;
            this.factoryProvider = factoryProvider;
            Editor.Roots.Subscribe(roots => LoadRoot(roots));
        }

        private void LoadRoot(IStateViewModel[] vmRoots)
        {
            if (vmRoots == null)
            {
                rootProperty.Value = null;
                return;
            }

            var interpreter = new ViewInterpreter(factoryProvider.GetViewFactories().ToArray());
            var views = vmRoots.Select(vm => interpreter.InterpretAsView(vm)).ToArray();
            rootProperty.Value = new StateRootDataContext(views);

            errorSubscription?.Dispose();
            errorSubscription = rootProperty.Value.OnError.Subscribe(x =>
            {
                throw x;
            });
        }
    }
}
