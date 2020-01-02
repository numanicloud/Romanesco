using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.View
{
    class StateRootDataContext
    {
        public StateViewContext[] RootViews { get; }

        public StateRootDataContext(StateViewContext[] roots)
        {
            RootViews = roots;
        }
    }
}
