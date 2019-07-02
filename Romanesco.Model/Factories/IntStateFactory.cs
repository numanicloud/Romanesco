using Romanesco.Interface;
using Romanesco.Model.Common;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Factories {
    public class IntStateFactory : IStateFactory {
        public IFieldState InterpretAsState(object value, string title, StateInterpretFunc interpretRecursively) {
            return value is int n ? new IntState(n, title) : null;
        }
    }
}
