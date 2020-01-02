using Romanesco.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romanesco.View
{
    class RootViewLoader
    {
        public StateRootDataContext LoadRoot(IViewFactory[] factories, IStateViewModel[] vmRoots)
        {
            var interpreter = new ViewInterpreter(factories);
            var views = vmRoots.Select(vm => interpreter.InterpretAsView(vm)).ToArray();
            return new StateRootDataContext(views);
        }
    }
}
