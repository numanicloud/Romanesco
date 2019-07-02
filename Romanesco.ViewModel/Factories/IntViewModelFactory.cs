using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Interface;
using Romanesco.Model.States;

namespace Romanesco.ViewModel.Factories {
    public class IntViewModelFactory : Interface.IStateViewModelFactory {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively) {
            return state is IntState i ? new IntViewModel(i) : null;
        }
    }
}
