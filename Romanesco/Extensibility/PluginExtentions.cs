using Romanesco.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romanesco.Extensibility {
    class PluginExtentions {
        public PluginExtentions(IEnumerable<IStateFactory> stateFactories,
            IEnumerable<IStateViewModelFactory> stateViewModelFactories,
            IEnumerable<IViewFactory> viewFactories) {
            this.StateFactories = stateFactories.ToArray();
            this.StateViewModelFactories = stateViewModelFactories.ToArray();
            this.ViewFactories = viewFactories.ToArray();
        }

        public IStateFactory[] StateFactories { get; }
        public IStateViewModelFactory[] StateViewModelFactories { get; }
        public IViewFactory[] ViewFactories { get; }
    }
}
