using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Common.ViewModel.Interfaces
{
    public delegate IStateViewModel ViewModelInterpretFunc(IFieldState state);

    public interface IStateViewModelFactory
    {
        IStateViewModel? InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively);
    }
}
