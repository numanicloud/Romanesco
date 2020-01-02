﻿using Romanesco.Extensibility;
using Romanesco.Model;
using Romanesco.Sample;
using Romanesco.View;
using System.Reactive.Linq;
using System;

namespace Romanesco
{
    class StartUp
    {
        public MainDataContext LoadMainDataContext()
        {
            var loader = new PluginLoader(new Common.Utility.ProjectContext());
            var extensions = loader.Load("Plugins");

            var editor = new Editor();
            var roots = editor.LoadRoots(extensions.StateFactories, typeof(Project));

            var vm = new EditorViewModel(editor);
            var vmRoots = vm.LoadRoots(extensions.StateViewModelFactories, roots.States);

            var viewLoader = new RootViewLoader();
            var viewRoot = viewLoader.LoadRoot(extensions.ViewFactories, vmRoots);
            var main = new MainDataContext(vm);
            main.Root.Value = viewRoot;

            foreach (var view in viewRoot.RootViews)
            {
                view.OnError.Subscribe(ex =>
                {
                    throw ex;
                });
            }

            return main;
        }
    }
}