using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;

namespace Romanesco.View.States
{
    public class NoneViewFactory : IViewFactory
    {
        public StateViewContext? InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is NoneViewModel vm)
            {
                var control = new NoneView() { DataContext = vm };
                return new StateViewContext(control, control, vm);
            }
            return null;
        }
    }
}
