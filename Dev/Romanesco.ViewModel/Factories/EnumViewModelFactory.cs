using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.States;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Factories
{
    public class EnumViewModelFactory : IStateViewModelFactory
    {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
        {
            if (state is EnumState @enum)
            {
                return new EnumViewModel(@enum);
            }
            return null;
        }
    }
}
