using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.Common.Utility
{
    public class StateViewContext
    {
        public UserControl InlineControl { get; }
        public UserControl BlockControl { get; }

        public StateViewContext(UserControl inlineControl, UserControl blockControl)
        {
            InlineControl = inlineControl;
            BlockControl = blockControl;
        }
    }
}
