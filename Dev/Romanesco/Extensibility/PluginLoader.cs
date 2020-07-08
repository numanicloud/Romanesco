using Romanesco.Common.Extensibility.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Romanesco.Extensibility
{
	internal class PluginLoader
	{
		public PluginExtentions Load(string dirPath)
		{
			var plugins = new List<IPluginService>();

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

				var type = asm.GetTypes().Where(x => typeof(IPluginService).IsAssignableFrom(x))
					.Where(x => !x.IsInterface && !x.IsAbstract)
					.FirstOrDefault();
				if (type == null)
				{
					continue;
				}

				if (Activator.CreateInstance(type) is IPluginService service)
				{
					plugins.Add(service);
				}
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

		private string? GetEntryPointAssemblyPath(string dirPath)
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
