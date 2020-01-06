using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Common;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.States;

namespace Romanesco.ViewModel.Factories
{
    public class NoneViewModelFactory : IStateViewModelFactory
    {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
        {
            return state is NoneState none ? new NoneViewModel(none) : null;
        }
    }
}
