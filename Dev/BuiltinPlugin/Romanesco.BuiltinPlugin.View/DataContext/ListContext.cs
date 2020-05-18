using Reactive.Bindings;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.ViewModel.States;
using System.Windows.Controls;

namespace Romanesco.BuiltinPlugin.View.DataContext
{
    public class ListContext
    {
        public ListViewModel ViewModel { get; }
        public ReadOnlyReactiveCollection<StateViewContext> Elements { get; }
        public ReactiveProperty<int> SelectedIndex { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<UserControl?> SelectedControl { get; } = new ReactiveProperty<UserControl?>();
        public IReadOnlyReactiveProperty<string> Title => ViewModel.Title;

        public ListContext(ListViewModel viewModel, ViewInterpretFunc interpreter)
        {
            ViewModel = viewModel;
            SelectedIndex.Value = -1;
            Elements = viewModel.Elements.ToReadOnlyReactiveCollection(vm => interpreter(vm));
        }
    }
}
