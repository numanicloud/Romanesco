using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.View
{
    class MainDataContext
    {
        public EditorViewModel Editor { get; }
        public ReactiveProperty<StateRootDataContext> Root { get; }

        public MainDataContext(EditorViewModel editor)
        {
            Editor = editor;
            Root = new ReactiveProperty<StateRootDataContext>();
        }
    }
}
