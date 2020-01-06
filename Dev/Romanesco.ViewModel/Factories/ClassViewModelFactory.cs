using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.States;
using Romanesco.ViewModel.States;
using System.Linq;

namespace Romanesco.ViewModel.Factories
{
    public class ClassViewModelFactory : IStateViewModelFactory
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
