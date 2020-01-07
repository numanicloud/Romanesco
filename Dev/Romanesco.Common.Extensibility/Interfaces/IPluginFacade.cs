using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.Common.Extensibility.Interfaces
{
    public interface IPluginFacade : IStateFactoryProvider, IStateViewModelFactoryProvider, IViewFactoryProvider
    {
    }
}
