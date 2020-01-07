using System.Collections.Generic;
using Romanesco.Common.Model.Basics;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.Common.ViewModel.Interfaces
{
    public interface IStateViewModelFactoryProvider
    {
        IEnumerable<IStateViewModelFactory> GetStateViewModelFactories();
    }
}
