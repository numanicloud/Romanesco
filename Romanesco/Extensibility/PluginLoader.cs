using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Linq;
using Romanesco.Common;
using System.Diagnostics;
using Romanesco.Common.Utility;

namespace Romanesco.Extensibility
{
    class PluginLoader
    {
        private readonly ProjectContext projectContext;

        public PluginLoader(ProjectContext projectContext)
        {
            this.projectContext = projectContext;
        }

        public PluginExtentions Load(string dirPath)
        {
            List<IStateFactory> stateFactories = new List<IStateFactory>();
            List<IStateViewModelFactory> stateViewModelFactories = new List<IStateViewModelFactory>();
            List<IViewFactory> viewFactories = new List<IViewFactory>();

            if (Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var directories = Directory.EnumerateDirectories(dirPath);
            foreach (var dir in directories)
            {
                var assemblyPath = GetEntryPointAssemblyPath(dir);
                if (assemblyPath == null)
                {
                    continue;
                }

                var asm = LoadPluginAssembly(Path.Combine(dir, assemblyPath));

                var type = asm.GetTypes().Where(x => typeof(IPluginFacade).IsAssignableFrom(x))
                    .Where(x => !x.IsInterface && !x.IsAbstract)
                    .FirstOrDefault();
                if (type == null)
                {
                    continue;
                }

                var facade = Activator.CreateInstance(type) as IPluginFacade;
                facade.LoadContext(projectContext);
                stateFactories.AddRange(facade.GetStateFactories());
                stateViewModelFactories.AddRange(facade.GetStateViewModelFactories());
                viewFactories.AddRange(facade.GetViewFactories());
            }

            Debug.Print("Loaded assemblies:");
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Debug.Print(asm.FullName);
            }

            return new PluginExtentions(stateFactories, stateViewModelFactories, viewFactories);
        }

        private Assembly LoadPluginAssembly(string assemblyPath)
        {
            return Assembly.LoadFrom(assemblyPath);
        }

        private Assembly LoadPluginAssemblyWithContext(string assemblyPath)
        {
            PluginLoadContext loadContext = new PluginLoadContext(assemblyPath);
            var name = new AssemblyName(Path.GetFileNameWithoutExtension(assemblyPath));
            return loadContext.LoadFromAssemblyName(name);
        }

        private string GetEntryPointAssemblyPath(string dirPath)
        {
            var settingPath = Path.Combine(dirPath, "Plugin.txt");
            if (!File.Exists(settingPath)) {
                return null;
            }

            using (var file = File.OpenRead(settingPath)) {
                using (var reader = new StreamReader(file)) {
                    return reader.ReadLine();
                }
            }
        }
    }
}
