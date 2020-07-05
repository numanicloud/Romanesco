using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.ViewModel.States
{
	class ViewModelInterpreter
    {
        private readonly IStateViewModelFactory[] factories;

        public ViewModelInterpreter(IEnumerable<IStateViewModelFactory> factories)
        {
            this.factories = factories.ToArray();
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
            return new NoneViewModel(new NoneState());
        }
    }
}
