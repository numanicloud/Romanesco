using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Romanesco.Interface;
using Romanesco.ViewModel;

namespace Romanesco.View.Factories {
    public class IntViewFactory : Interface.IViewFactory {
        public UserControl InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively) {
            if (viewModel is IntViewModel vm) {
                var control = new IntView();
                control.DataContext = vm;
                return control;
            }
            return null;
        }
    }
}
