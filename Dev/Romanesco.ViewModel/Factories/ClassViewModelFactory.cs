using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romanesco.Common;
using Romanesco.Model.States;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Factories
{
    public class ClassViewModelFactory : Common.IStateViewModelFactory
    {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
        {
            if (state is ClassState @class)
            {
                var fields = @class.Fields.Select(x => interpretRecursively(x)).ToArray();
                return new ClassViewModel(@class, fields);
            }
            return null;
        }
    }
}
