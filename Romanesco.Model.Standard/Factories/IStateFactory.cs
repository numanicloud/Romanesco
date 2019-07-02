using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Factories {
    public interface IStateFactory {
        IFieldState InterpretAsState<T>(T value, string title, Span<IStateFactory> rest) {

        }
    }
}
