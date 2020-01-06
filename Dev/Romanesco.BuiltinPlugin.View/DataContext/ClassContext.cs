using Reactive.Bindings;
using Romanesco.Common.View.Basics;
using Romanesco.ViewModel.States;
using System.Windows.Controls;

namespace Romanesco.BuiltinPlugin.View.DataContext
{
    public class ClassContext
    {
        public StateViewContext[] ChildViews { get; }
        public ReactiveProperty<UserControl> ClosedUpView { get; set; }
        public ClassViewModel ViewModel { get; set; }
        public IReadOnlyReactiveProperty<string> Title => ViewModel.Title;

        public ClassContext(ClassViewModel viewModel, StateViewContext[] childViews)
        {
            ViewModel = viewModel;
            ChildViews = childViews;
            ClosedUpView = new ReactiveProperty<UserControl>((UserControl)null);
        }
    }
}
