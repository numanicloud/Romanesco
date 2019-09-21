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
    public class ArrayContext
    {
        public ListViewModel ViewModel { get; set; }
        public ReactiveCollection<StateViewContext> Elements { get; }
        public ReactiveProperty<int> SelectedIndex { get; set; } = new ReactiveProperty<int>();
        public ReactiveProperty<UserControl> SelectedControl { get; set; }

        public ArrayContext(ListViewModel viewModel)
        {
            ViewModel = viewModel;
            Elements = new ReactiveCollection<StateViewContext>();
        }
    }
}
