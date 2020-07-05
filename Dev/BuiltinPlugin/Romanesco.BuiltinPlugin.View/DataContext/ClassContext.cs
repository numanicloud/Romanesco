using Reactive.Bindings;
using Romanesco.Common.View.Basics;
using System.Windows.Controls;
using Romanesco.BuiltinPlugin.ViewModel.States;

namespace Romanesco.BuiltinPlugin.View.DataContext
{
    public class ClassContext
    {
        public StateViewContext[] ChildViews { get; }
        public ReactiveProperty<UserControl?> ClosedUpView { get; set; }
        public ClassViewModel ViewModel { get; set; }
        public IReadOnlyReactiveProperty<string> Title => ViewModel.Title;

        public ClassContext(ClassViewModel viewModel, StateViewContext[] childViews)
        {
            ViewModel = viewModel;
            ChildViews = childViews;
            ClosedUpView = new ReactiveProperty<UserControl?>((UserControl?)null);
        }
    }
}
