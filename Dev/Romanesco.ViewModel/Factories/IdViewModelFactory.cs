﻿using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.ViewModel.Factories
{
    public class IdViewModelFactory : IStateViewModelFactory
    {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
        {
            if (state is Model.States.IntIdChoiceState intid)
            {
                return new States.IntIdChoiceViewModel(intid);
            }
            return null;
        }
    }
}
