﻿using Romanesco.Common.Extensibility.Interfaces;
using Romanesco.Common.Model.Basics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Romanesco.Extensibility
{
    class PluginLoader
    {
        public PluginExtentions Load(string dirPath)
        {
            var plugins = new List<IPluginFacade>();

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
                plugins.Add(facade);
            }

            Debug.Print("Loaded assemblies:");
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Debug.Print(asm.FullName);
            }

            return new PluginExtentions(plugins.ToArray());
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
