using Romanesco.Interface;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model {
    public class ObjectInterpreter {
        public ObjectInterpreter(IStateFactory[] factories) {
            Factories = factories;
        }

        public IStateFactory[] Factories { get; }

        public IFieldState InterpretAsState(object instance, string title) {
            foreach (var factory in Factories) {
                var result = factory.InterpretAsState(instance, title, InterpretAsState);
                if (result != null) {
                    return result;
                }
            }
            return new NoneState();
        }
    }
}
