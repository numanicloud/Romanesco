using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.ViewModel.States;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.View.DataContext
{
    public class ListContext
    {
        public ListViewModel ViewModel { get; }
        public ReadOnlyReactiveCollection<StateViewContext> Elements { get; }
        public ReactiveProperty<int> SelectedIndex { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<UserControl> SelectedControl { get; } = new ReactiveProperty<UserControl>();
        public ReactiveProperty<string> Title => ViewModel.Title;

        public ListContext(ListViewModel viewModel, ViewInterpretFunc interpreter)
        {
            ViewModel = viewModel;
            SelectedIndex.Value = -1;
            Elements = viewModel.Elements.ToReadOnlyReactiveCollection(vm => interpreter(vm));
        }
    }
}
