using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.View.States
{
	internal class ViewInterpreter
    {
		private readonly IEnumerable<IRootViewFactory> _rootViewFactories;
		private readonly IViewFactory[] _factories;

        public ViewInterpreter(IEnumerable<IViewFactory> factories, IEnumerable<IRootViewFactory> rootViewFactories)
		{
			_rootViewFactories = rootViewFactories;
			this._factories = factories.ToArray();
		}

		public StateViewContext InterpretAsView(IStateViewModel viewModel)
		{
			foreach (var factory in _factories)
            {
                var result = factory.InterpretAsView(viewModel, InterpretAsView);
                if (result != null)
                {
					return result;
                }
            }

			return new StateViewContext(new NoneView(), new NoneView(), new NoneViewModel(new NoneState()));
        }

		public UserControl InterpretAsControl(IStateViewModel viewModel)
		{
			foreach (var factory in _rootViewFactories)
			{
				var result = factory.Interpret(viewModel);
				if (result is not null)
				{
					return result;
				}
			}

			return new NoneView();
		}
    }
}
