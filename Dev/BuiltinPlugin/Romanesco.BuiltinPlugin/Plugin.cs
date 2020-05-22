using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.BuiltinPlugin.ViewModel.Factories;
using Romanesco.Common.Extensibility.Interfaces;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.ProjectComponents;
using Romanesco.ViewModel.Factories;
using System.Collections.Generic;

namespace Romanesco.BuiltinPlugin
{
    public class Plugin : IPluginFacade
    {
        public IEnumerable<IStateFactory> GetStateFactories(ProjectContextCrawler context)
        {
            var masterListContext = new MasterListContext();
            yield return new IdStateFactory(masterListContext);
            yield return new PrimitiveStateFactory(context.CommandHistory);
            yield return new EnumStateFactory(context.CommandHistory);
            yield return new ListStateFactory(masterListContext, context.CommandHistory);
            yield return new SubtypingStateFactory();
            yield return new ClassStateFactory();
        }

        public IEnumerable<IStateViewModelFactory> GetStateViewModelFactories()
        {
            yield return new IdViewModelFactory();
            yield return new PrimitiveViewModelFactory();
            yield return new EnumViewModelFactory();
            yield return new SubtypingClassViewModelFactory();
            yield return new ClassViewModelFactory();
            yield return new ListViewModelFactory();
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
