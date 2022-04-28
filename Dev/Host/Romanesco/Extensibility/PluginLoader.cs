using Romanesco.Common.Extensibility.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Numani.TypedFilePath;
using Numani.TypedFilePath.Interfaces;

namespace Romanesco.Extensibility
{
	internal class PluginLoader
	{
		public PluginExtentions Load(IRelativeDirectoryPath dirPath)
		{
			var plugins = new List<IPluginService>();

			if (!dirPath.Exists())
			{
				dirPath.Create();
			}

			var loadContext = AssemblyLoadContext.Default;
			loadContext.Resolving += LoadContext_Resolving;

			var directories = Directory.EnumerateDirectories(dirPath.PathString);
			foreach (var dir in directories)
			{
				if (dir.AsDirectoryPath() is not IRelativeDirectoryPath pluginDirPath)
				{
					continue;
				}

				var assemblyPath = GetEntryPointAssemblyPath(pluginDirPath);
				if (assemblyPath == null)
				{
					continue;
				}

				var asm = loadContext.LoadFromAssemblyPath(assemblyPath.PathString);

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

			return new PluginExtentions(plugins.ToArray());
		}

		private Assembly? LoadContext_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
		{
			var fileName = (arg2.Name?.Split(',')[0] + ".dll").AsFilePath();
			if (fileName is not IRelativeFilePath relativeFilePath)
			{
				return null;
			}

			if (!Directory.Exists("Plugins"))
			{
				return null;
			}

			var dirs = Directory.EnumerateDirectories("Plugins");
			foreach (var dir in dirs)
			{
				if (TypedPath.AsDirectoryPath(dir) is not IRelativeDirectoryPath directoryPath)
				{
					continue;
				}

				try
				{
					var path = TypedPath.GetCurrentDirectory()
						.Combine(directoryPath)
						.Combine(relativeFilePath);
					var asm = arg1.LoadFromAssemblyPath(path.PathString);
					return asm;
				}
				catch (Exception e)
				{
					Debug.WriteLine(e);
				}
			}

			return null;
		}

		private Assembly LoadPluginAssembly(string assemblyPath)
		{
			return Assembly.LoadFrom(assemblyPath);
		}

		private IAbsoluteFilePath? GetEntryPointAssemblyPath(IRelativeDirectoryPath dirPath)
		{
			if (TypedPath.AsFilePath("Plugin.txt") is not IRelativeFilePath filePath)
			{
				return null;
			}

			var settingPath = dirPath.Combine(filePath);
			if (!settingPath.Exists())
			{
				return null;
			}

			using var file = settingPath.OpenRead();
			using var reader = new StreamReader(file);

			return reader.ReadLine()?.AsFilePath() is IRelativeFilePath relative
				? TypedPath.GetCurrentDirectory().Combine(dirPath.Combine(relative))
				: throw new Exception();
		}
	}
}
