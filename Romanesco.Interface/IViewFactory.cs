using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.Interface {
    public delegate UserControl ViewInterpretFunc(IStateViewModel viewModel);

    public interface IViewFactory {
        UserControl InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively);
    }
}
