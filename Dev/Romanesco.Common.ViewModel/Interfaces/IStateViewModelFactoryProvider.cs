using System.Collections.Generic;

namespace Romanesco.Common.ViewModel.Interfaces
{
    public interface IStateViewModelFactoryProvider
    {
        IEnumerable<IStateViewModelFactory> GetStateViewModelFactories();
    }
}
