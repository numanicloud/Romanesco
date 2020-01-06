﻿using Romanesco.Common.Extensibility.Interfaces;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.Infrastructure;
using System.Collections.Generic;

namespace Romanesco.BuiltinPlugin
{
    public class Plugin : IPluginFacade
    {
        private ProjectContext projectContext;

        public IEnumerable<IStateFactory> GetStateFactories()
        {
            var masterListContext = new MasterListContext();
            yield return new Model.Factories.IdStateFactory(masterListContext);
            yield return new Model.Factories.PrimitiveStateFactory(projectContext.CommandHistory);
            yield return new Model.Factories.EnumStateFactory(projectContext.CommandHistory);
            yield return new Model.Factories.ListStateFactory(masterListContext, projectContext.CommandHistory);
            yield return new Model.Factories.ClassStateFactory();
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

        public void LoadContext(ProjectContext context)
        {
            projectContext = context;
        }
    }
}
