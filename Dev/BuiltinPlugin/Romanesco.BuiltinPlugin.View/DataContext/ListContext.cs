using Reactive.Bindings;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using System.Windows.Controls;
using Romanesco.BuiltinPlugin.ViewModel.States;

namespace Romanesco.BuiltinPlugin.View.DataContext
{
    public class ListContext
    {
        public ListViewModel ViewModel { get; }
        public ReadOnlyReactiveCollection<StateViewContext> Elements { get; }
        public IReactiveProperty<int> SelectedIndex { get; }
        public ReactiveProperty<UserControl?> SelectedControl { get; } = new();
        public IReadOnlyReactiveProperty<string> Title => ViewModel.Title;

        public ListContext(ListViewModel viewModel, ViewInterpretFunc interpreter, IReactiveProperty<int> selectedIndex)
        {
            ViewModel = viewModel;
            SelectedIndex = selectedIndex;
            Elements = viewModel.Elements.ToReadOnlyReactiveCollection(vm => interpreter(vm));
        }
    }
}
