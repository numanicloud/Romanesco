using System;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Common.Model.Basics
{
    public sealed class StateRoot : IDisposable
    {
        public object RootInstance { get; }
        public IFieldState[] States { get; }

        public StateRoot(object rootInstance, IFieldState[] states)
        {
            RootInstance = rootInstance;
            States = states;
        }

        public void Dispose()
        {
	        foreach (var fieldState in States)
	        {
		        fieldState.Dispose();
	        }
        }
    }
}
