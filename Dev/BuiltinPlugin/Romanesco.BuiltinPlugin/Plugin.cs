using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Extensibility.Interfaces;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.ProjectComponents;
using System.Collections.Generic;

namespace Romanesco.BuiltinPlugin
{
    public class Plugin : IPluginFacade
    {
        public IEnumerable<IStateFactory> GetStateFactories(ProjectContextCrawler context)
        {
            var masterListContext = new MasterListContext();
            yield return new Model.Factories.IdStateFactory(masterListContext);
            yield return new Model.Factories.PrimitiveStateFactory(context.CommandHistory);
            yield return new Model.Factories.EnumStateFactory(context.CommandHistory);
            yield return new Model.Factories.ListStateFactory(masterListContext, context.CommandHistory);
            yield return new SubtypingStateFactory();
            yield return new ClassStateFactory();
        }

        public IEnumerable<IStateViewModelFactory> GetStateViewModelFactories()
        {
            yield return new ViewModel.Factories.IdViewModelFactory();
            yield return new ViewModel.Factories.PrimitiveViewModelFactory();
            yield return new ViewModel.Factories.EnumViewModelFactory();
            yield return new ViewModel.Factories.ClassViewModelFactory();
            yield return new ViewModel.Factories.ListViewModelFactory();
        }

        public IEnumerable<IViewFactory> GetViewFactories()
        {
            yield return new View.Factories.IdViewFactory();
            yield return new View.Factories.PrimitiveViewFactory();
            yield return new View.Factories.EnumViewFactory();
            yield return new View.Factories.ClassViewFactory();
            yield return new View.Factories.ArrayViewFactory();
        }
    }
}
