using Romanesco.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.BuiltinPlugin {
    public class Plugin : IPluginFacade {
        public IEnumerable<IStateFactory> GetStateFactories() {
            yield return new Model.Factories.IntStateFactory();
        }

        public IEnumerable<IStateViewModelFactory> GetStateViewModelFactories() {
            yield return new ViewModel.Factories.IntViewModelFactory();
        }

        public IEnumerable<IViewFactory> GetViewFactories() {
            yield return new View.Factories.IntViewFactory();
        }
    }
}
