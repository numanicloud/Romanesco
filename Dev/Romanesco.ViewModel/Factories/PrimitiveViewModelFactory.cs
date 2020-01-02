using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Common;
using Romanesco.Model.States;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Factories
{
    public class PrimitiveViewModelFactory : Common.IStateViewModelFactory
    {
        public IStateViewModel InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
        {
            switch(state)
            {
            case IntState i: return new IntViewModel(i);
            case FloatState f: return new FloatViewModel(f);
            case StringState s: return new StringViewModel(s);
            case BoolState b: return new BoolViewModel(b);
            case ByteState b: return new ByteViewModel(b);
            case ShortState s: return new ShortViewModel(s);
            case LongState l: return new LongViewModel(l);
            case DoubleState d: return new DoubleViewModel(d);
            default: return null;
            }
        }
    }
}
