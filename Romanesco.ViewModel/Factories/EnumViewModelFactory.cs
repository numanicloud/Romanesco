using Romanesco.Common;
using Romanesco.Model.States;
using Romanesco.ViewModel.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.ViewModel.Factories
{
    public class EnumViewModelFactory : IStateViewModelFactory
    {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
        {
            if (state is EnumState @enum)
            {
                return new EnumViewModel(@enum);
            }
            return null;
        }
    }
}
