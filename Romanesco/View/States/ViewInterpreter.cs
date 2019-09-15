using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.View {
    class ViewInterpreter {
        private readonly IViewFactory[] factories;

        public ViewInterpreter(IViewFactory[] factories) {
            this.factories = factories;
        }

        public StateViewContext InterpretAsView(IStateViewModel viewModel) {
            foreach (var factory in factories) {
                var result = factory.InterpretAsView(viewModel, InterpretAsView);
                if (result != null) {
                    return result;
                }
            }
            return new StateViewContext(new NoneView(), new NoneView(), new NoneViewModel(new Model.States.NoneState()));
        }
    }
}
