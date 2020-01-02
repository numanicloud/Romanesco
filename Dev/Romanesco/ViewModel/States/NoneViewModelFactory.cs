using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Common;
using Romanesco.Model.States;

namespace Romanesco.ViewModel.Factories {
    public class NoneViewModelFactory : Common.IStateViewModelFactory {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively) {
            return state is NoneState none ? new NoneViewModel(none) : null;
        }
    }
}
