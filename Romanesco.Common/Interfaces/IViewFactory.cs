using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.Common {
    public delegate object ViewInterpretFunc(IStateViewModel viewModel);

    public interface IViewFactory {
        object InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively);
    }
}
