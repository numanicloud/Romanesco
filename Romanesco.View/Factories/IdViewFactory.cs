using Romanesco.Common;
using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.View.Factories
{
    public class IdViewFactory : Common.IViewFactory
    {
        public StateViewContext InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is ViewModel.States.IntIdChoiceViewModel intid)
            {
                var inline = new View.IdChoiceView() { DataContext = intid };
                var block = new View.IdChoiceView() { DataContext = intid };
                return new StateViewContext(inline, block, intid);
            }
            return null;
        }
    }
}
