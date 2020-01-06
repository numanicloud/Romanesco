using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.Model.States
{
    public class IntState : PrimitiveTypeState<int>
    {
        public IntState(ValueStorage settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class ByteState : PrimitiveTypeState<byte>
    {
        public ByteState(ValueStorage settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class ShortState : PrimitiveTypeState<short>
    {
        public ShortState(ValueStorage settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class LongState : PrimitiveTypeState<long>
    {
        public LongState(ValueStorage settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class FloatState : PrimitiveTypeState<float>
    {
        public FloatState(ValueStorage settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class DoubleState : PrimitiveTypeState<double>
    {
        public DoubleState(ValueStorage settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class StringState : PrimitiveTypeState<string>
    {
        public StringState(ValueStorage settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class BoolState : PrimitiveTypeState<bool>
    {
        public BoolState(ValueStorage settability, CommandHistory history) : base(settability, history)
        {
        }
    }
}
