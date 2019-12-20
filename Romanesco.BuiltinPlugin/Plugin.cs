using Romanesco.Common;
using Romanesco.Model.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.BuiltinPlugin
{
    public class Plugin : IPluginFacade
    {
        private readonly MasterListContext masterListContext;

        public Plugin()
        {
            masterListContext = new MasterListContext();
        }

        public IEnumerable<IStateFactory> GetStateFactories() {
            yield return new Model.Factories.IdStateFactory(masterListContext);
            yield return new Model.Factories.PrimitiveStateFactory();
            yield return new Model.Factories.ListStateFactory(masterListContext);
            yield return new Model.Factories.ClassStateFactory();
        }

        public IEnumerable<IStateViewModelFactory> GetStateViewModelFactories() {
            yield return new ViewModel.Factories.IdViewModelFactory();
            yield return new ViewModel.Factories.PrimitiveViewModelFactory();
            yield return new ViewModel.Factories.ClassViewModelFactory();
            yield return new ViewModel.Factories.ListViewModelFactory();
        }

        public IEnumerable<IViewFactory> GetViewFactories() {
            yield return new View.Factories.IdViewFactory();
            yield return new View.Factories.PrimitiveViewFactory();
            yield return new View.Factories.ClassViewFactory();
            yield return new View.Factories.ArrayViewFactory();
        }
    }
}
