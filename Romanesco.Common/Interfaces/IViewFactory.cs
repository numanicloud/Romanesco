using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.Common {
    public delegate StateViewContext ViewInterpretFunc(IStateViewModel viewModel);

    public interface IViewFactory {
        StateViewContext InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively);
    }
}
