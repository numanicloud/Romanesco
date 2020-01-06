using Romanesco.Common.View.Basics;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.Common.View.Interfaces
{
    public delegate StateViewContext ViewInterpretFunc(IStateViewModel viewModel);

    public interface IViewFactory
    {
        StateViewContext InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively);
    }
}
