using Romanesco.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.ViewModel {
    class ViewModelInterpreter {
        private readonly IStateViewModelFactory[] factories;

        public ViewModelInterpreter(IStateViewModelFactory[] factories) {
            this.factories = factories;
        }

        public IStateViewModel InterpretAsViewModel(IFieldState state) {
            foreach (var factory in factories) {
                var result = factory.InterpretAsViewModel(state, InterpretAsViewModel);
                if (result != null) {
                    return result;
                }
            }
            return new NoneViewModel(new Model.States.NoneState());
        }
    }
}
