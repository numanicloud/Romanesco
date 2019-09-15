using Romanesco.Common;
using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.States
{
    public class IntState : PrimitiveTypeState<int>
    {
        public IntState(ValueSettability settability) : base(settability)
        {
        }
    }

    public class ByteState : PrimitiveTypeState<byte>
    {
        public ByteState(ValueSettability settability) : base(settability)
        {
        }
    }

    public class ShortState : PrimitiveTypeState<short>
    {
        public ShortState(ValueSettability settability) : base(settability)
        {
        }
    }

    public class LongState : PrimitiveTypeState<long>
    {
        public LongState(ValueSettability settability) : base(settability)
        {
        }
    }

    public class FloatState : PrimitiveTypeState<float>
    {
        public FloatState(ValueSettability settability) : base(settability)
        {
        }
    }

    public class DoubleState : PrimitiveTypeState<double>
    {
        public DoubleState(ValueSettability settability) : base(settability)
        {
        }
    }

    public class StringState : PrimitiveTypeState<string>
    {
        public StringState(ValueSettability settability) : base(settability)
        {
        }
    }

    public class BoolState : PrimitiveTypeState<bool>
    {
        public BoolState(ValueSettability settability) : base(settability)
        {
        }
    }
}
