using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Interface {
    public delegate IStateViewModel ViewModelInterpretFunc(IFieldState state);

    public interface IStateViewModelFactory {
        IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively);
    }
}
