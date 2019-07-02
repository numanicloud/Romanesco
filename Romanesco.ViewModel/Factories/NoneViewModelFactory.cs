using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Interface;
using Romanesco.Model.States;

namespace Romanesco.ViewModel.Factories {
    public class NoneViewModelFactory : Interface.IStateViewModelFactory {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively) {
            return state is NoneState none ? new NoneViewModel(none) : null;
        }
    }
}
