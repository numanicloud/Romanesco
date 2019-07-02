using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Linq;
using Romanesco.Interface;

namespace Romanesco.Extensibility {
    class PluginLoader {
        public PluginExtentions Load(string dirPath) {
            List<IStateFactory> stateFactories = new List<IStateFactory>();
            List<IStateViewModelFactory> stateViewModelFactories = new List<IStateViewModelFactory>();
            List<IViewFactory> viewFactories = new List<IViewFactory>();

            var directories = Directory.EnumerateDirectories(dirPath);
            foreach (var dir in directories) {
                var settingPath = Path.Combine(dir, "Plugin.txt");
                if (!File.Exists(settingPath)) {
                    continue;
                }

                string assemblyPath;  // Relative
                using (var file = File.OpenRead(settingPath)) {
                    using (var reader = new StreamReader(file)) {
                        assemblyPath = reader.ReadLine();
                    }
                }

                var asm = Assembly.LoadFrom(Path.Combine(dir, assemblyPath));
                var type = asm.GetTypes().Where(x => typeof(IPluginFacade).IsAssignableFrom(x))
                    .Where(x => !x.IsInterface && !x.IsAbstract)
                    .FirstOrDefault();
                if (type == null) {
                    continue;
                }

                var facade = Activator.CreateInstance(type) as IPluginFacade;
                stateFactories.AddRange(facade.GetStateFactories());
                stateViewModelFactories.AddRange(facade.GetStateViewModelFactories());
                viewFactories.AddRange(facade.GetViewFactories());
            }

            return new PluginExtentions(stateFactories, stateViewModelFactories, viewFactories);
        }
    }
}
