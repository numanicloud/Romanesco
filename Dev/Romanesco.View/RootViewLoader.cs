using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System.Linq;

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
