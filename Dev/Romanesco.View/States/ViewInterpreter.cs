using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;
using System.Collections.Generic;
using System.Linq;

namespace Romanesco.View.States
{
    class ViewInterpreter
    {
        private readonly IViewFactory[] factories;

        public ViewInterpreter(IEnumerable<IViewFactory> factories)
        {
            this.factories = factories.ToArray();
        }

        public StateViewContext InterpretAsView(IStateViewModel viewModel)
        {
            foreach (var factory in factories)
            {
                var result = factory.InterpretAsView(viewModel, InterpretAsView);
                if (result != null)
                {
                    return result;
                }
            }
            return new StateViewContext(new NoneView(), new NoneView(), new NoneViewModel(new Model.States.NoneState()));
        }
    }
}
