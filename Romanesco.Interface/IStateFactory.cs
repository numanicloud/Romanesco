using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Interface {

    public delegate IFieldState StateInterpretFunc(object value, string title);

    public interface IStateFactory {
        IFieldState InterpretAsState(object value, string title, StateInterpretFunc interpretRecursively);
    }
}
