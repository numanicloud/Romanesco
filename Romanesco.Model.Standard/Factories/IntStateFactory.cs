using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Factories {
    public class IntStateFactory {
        public IFieldState InterpretAsState<T>(T value, string title) {
            if (value is int n) {
                return new IntState(n, title);
            } else {

            }
        }
    }
}
