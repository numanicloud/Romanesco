using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Common;
using Romanesco.Model.States;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Factories
{
    public class ListViewModelFactory : IStateViewModelFactory
    {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
        {
            if (state is ListState array)
            {
                return new ListViewModel(array, interpretRecursively);
            }
            return null;
        }
    }
}
