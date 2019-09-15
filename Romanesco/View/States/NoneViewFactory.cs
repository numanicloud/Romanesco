using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Romanesco.Common;
using Romanesco.ViewModel;

namespace Romanesco.View.Factories {
    public class NoneViewFactory : IViewFactory {
        public object InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively) {
            if (viewModel is NoneViewModel vm) {
                var control = new NoneView() { DataContext = vm };
                return new Common.Utility.StateViewContext(control, control);
            }
            return null;
        }
    }
}
