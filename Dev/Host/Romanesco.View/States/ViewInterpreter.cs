using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;
using System.Collections.Generic;
using System.Linq;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.View.States
{
	internal class ViewInterpreter
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
            return new StateViewContext(new NoneView(), new NoneView(), new NoneViewModel(new NoneState()));
        }
    }
}
