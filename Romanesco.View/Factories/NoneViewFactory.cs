using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Romanesco.Interface;
using Romanesco.ViewModel;

namespace Romanesco.View.Factories {
    public class NoneViewFactory : Interface.IViewFactory {
        public UserControl InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively) {
            if (viewModel is NoneViewModel vm) {
                return new NoneView() { DataContext = vm };
            }
            return null;
        }
    }
}
