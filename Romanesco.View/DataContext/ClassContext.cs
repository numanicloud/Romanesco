using Reactive.Bindings;
using Romanesco.Common.Utility;
using Romanesco.ViewModel.States;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.View.DataContext
{
    public class ClassContext
    {
        public StateViewContext[] ChildViews { get; }
        public ReactiveProperty<UserControl> ClosedUpView { get; set; }
        public ClassViewModel ViewModel { get; set; }
        public ReactiveProperty<string> Title => ViewModel.Title;

        public ClassContext(ClassViewModel viewModel, StateViewContext[] childViews)
        {
            ViewModel = viewModel;
            ChildViews = childViews;
            ClosedUpView = new ReactiveProperty<UserControl>((UserControl)null);
        }
    }
}
