using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.View.View;
using Romanesco.ViewModel.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.View.Factories
{
    public class EnumViewFactory : IViewFactory
    {
        public StateViewContext InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is EnumViewModel @enum)
            {
                var inline = new EnumView() { DataContext = @enum };
                var block = new EnumView() { DataContext = @enum };
                return new StateViewContext(inline, block, @enum);
            }
            return null;
        }
    }
}
