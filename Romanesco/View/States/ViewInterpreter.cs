using Romanesco.Interface;
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

        public UserControl InterpretAsView(IStateViewModel viewModel) {
            foreach (var factory in factories) {
                var result = factory.InterpretAsView(viewModel, InterpretAsView);
                if (result != null) {
                    return result;
                }
            }
            return new NoneView();
        }
    }
}
