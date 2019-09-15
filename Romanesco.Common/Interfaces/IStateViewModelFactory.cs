using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common {
    public delegate IStateViewModel ViewModelInterpretFunc(IFieldState state);

    public interface IStateViewModelFactory {
        IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively);
    }
}
