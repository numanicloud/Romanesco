﻿using Romanesco.Common.ViewModel.Implementations;
using Romanesco.Model.States;

namespace Romanesco.ViewModel.States
{
    public class IntViewModel : PrimitiveTypeViewModel<int>
    {
        public IntViewModel(IntState state) : base(state, state.PrimitiveContent)
        {
        }
    }

    public class ByteViewModel : PrimitiveTypeViewModel<byte>
    {
        public ByteViewModel(ByteState state) : base(state, state.PrimitiveContent)
        {
        }
    }

    public class ShortViewModel : PrimitiveTypeViewModel<short>
    {
        public ShortViewModel(ShortState state) : base(state, state.PrimitiveContent)
        {
        }
    }

    public class LongViewModel : PrimitiveTypeViewModel<long>
    {
        public LongViewModel(LongState state) : base(state, state.PrimitiveContent)
        {
        }
    }

    public class FloatViewModel : PrimitiveTypeViewModel<float>
    {
        public FloatViewModel(FloatState state) : base(state, state.PrimitiveContent)
        {
        }
    }

    public class DoubleViewModel : PrimitiveTypeViewModel<double>
    {
        public DoubleViewModel(DoubleState state) : base(state, state.PrimitiveContent)
        {
        }
    }

    public class StringViewModel : PrimitiveTypeViewModel<string>
    {
        public StringViewModel(StringState state) : base(state, state.PrimitiveContent)
        {
        }
    }

    public class BoolViewModel : PrimitiveTypeViewModel<bool>
    {
        public BoolViewModel(BoolState state) : base(state, state.PrimitiveContent)
        {
        }
    }
}
