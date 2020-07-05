using Romanesco.Common.Model.Implementations;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.ViewModel.States
{
    public class NoneViewModelFactory : IStateViewModelFactory
    {
        public IStateViewModel? InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
        {
            return state is NoneState none ? new NoneViewModel(none) : null;
        }
    }
}
