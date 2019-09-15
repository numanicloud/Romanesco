using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.ViewModel;

namespace Romanesco.View.Factories {
    public class NoneViewFactory : IViewFactory {
        public StateViewContext InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively) {
            if (viewModel is NoneViewModel vm) {
                var control = new NoneView() { DataContext = vm };
                return new StateViewContext(control, control, vm);
            }
            return null;
        }
    }
}
