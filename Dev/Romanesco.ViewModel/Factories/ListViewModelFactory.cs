using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.States;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Factories
{
    public class ListViewModelFactory : IStateViewModelFactory
    {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
        {
            if (state is ListState array)
            {
                return new ListViewModel(array, interpretRecursively);
            }
            return null;
        }
    }
}
