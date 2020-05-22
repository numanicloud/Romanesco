using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;

namespace Romanesco.BuiltinPlugin.View.Factories
{
    public class IdViewFactory : IViewFactory
    {
        public StateViewContext? InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is IntIdChoiceViewModel intid)
            {
                var inline = new View.IdChoiceView() { DataContext = intid };
                var block = new View.IdChoiceView() { DataContext = intid };
                return new StateViewContext(inline, block, intid);
            }
            return null;
        }
    }
}
