using Romanesco.Common;
using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.States
{
    public class IntState : PrimitiveTypeState<int>
    {
        public IntState(ValueSettability settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class ByteState : PrimitiveTypeState<byte>
    {
        public ByteState(ValueSettability settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class ShortState : PrimitiveTypeState<short>
    {
        public ShortState(ValueSettability settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class LongState : PrimitiveTypeState<long>
    {
        public LongState(ValueSettability settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class FloatState : PrimitiveTypeState<float>
    {
        public FloatState(ValueSettability settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class DoubleState : PrimitiveTypeState<double>
    {
        public DoubleState(ValueSettability settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class StringState : PrimitiveTypeState<string>
    {
        public StringState(ValueSettability settability, CommandHistory history) : base(settability, history)
        {
        }
    }

    public class BoolState : PrimitiveTypeState<bool>
    {
        public BoolState(ValueSettability settability, CommandHistory history) : base(settability, history)
        {
        }
    }
}
