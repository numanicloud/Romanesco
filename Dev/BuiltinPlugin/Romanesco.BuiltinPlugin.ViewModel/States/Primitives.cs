using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.ViewModel.Implementations;

namespace Romanesco.BuiltinPlugin.ViewModel.States
{
    public class IntViewModel : PrimitiveTypeViewModel<IntState, int>
    {
        public IntViewModel(IntState state) : base(state)
        {
        }
    }

    public class ByteViewModel : PrimitiveTypeViewModel<ByteState, byte>
    {
        public ByteViewModel(ByteState state) : base(state)
        {
        }
    }

    public class ShortViewModel : PrimitiveTypeViewModel<ShortState, short>
    {
        public ShortViewModel(ShortState state) : base(state)
        {
        }
    }

    public class LongViewModel : PrimitiveTypeViewModel<LongState, long>
    {
        public LongViewModel(LongState state) : base(state)
        {
        }
    }

    public class FloatViewModel : PrimitiveTypeViewModel<FloatState, float>
    {
        public FloatViewModel(FloatState state) : base(state)
        {
        }
    }

    public class DoubleViewModel : PrimitiveTypeViewModel<DoubleState, double>
    {
        public DoubleViewModel(DoubleState state) : base(state)
        {
        }
    }

    public class StringViewModel : PrimitiveTypeViewModel<StringState, string>
    {
        public StringViewModel(StringState state) : base(state)
        {
        }
    }

    public class BoolViewModel : PrimitiveTypeViewModel<BoolState, bool>
    {
        public BoolViewModel(BoolState state) : base(state)
        {
        }
    }
}
