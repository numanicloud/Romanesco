using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.ViewModel.States
{
    class ViewModelInterpreter
    {
        private readonly IStateViewModelFactory[] factories;

        public ViewModelInterpreter(IStateViewModelFactory[] factories)
        {
            this.factories = factories;
        }

        public IStateViewModel InterpretAsViewModel(IFieldState state)
        {
            foreach (var factory in factories)
            {
                var result = factory.InterpretAsViewModel(state, InterpretAsViewModel);
                if (result != null)
                {
                    return result;
                }
            }
            return new NoneViewModel(new Model.States.NoneState());
        }
    }
}
