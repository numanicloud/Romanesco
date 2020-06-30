using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Romanesco.Model.ProjectComponents
{
	public class MyAssemblyLoadContext : System.Runtime.Loader.AssemblyLoadContext
	{
		private readonly string pluginPath;

		public MyAssemblyLoadContext(string pluginDllPath) : base(isCollectible: true)
		{
			var fullPath = Path.GetFullPath(pluginDllPath);
			var fileName = Path.GetFileName(pluginDllPath);
			pluginPath = fullPath.Replace(fileName, "");
		}

		protected override Assembly? Load(AssemblyName assemblyName)
		{
			if (assemblyName.Name is string)
			{
				var path = Path.Combine(pluginPath, assemblyName.Name + ".dll");

				if (File.Exists(path))
				{
					if (assemblyName.ProcessorArchitecture != ProcessorArchitecture.MSIL)
					{
						var resolver = new PathAssemblyResolver(new[] { path, typeof(object).Assembly.Location });
						var metadataLoader = new MetadataLoadContext(resolver, assemblyName.Name);
						return metadataLoader.LoadFromAssemblyPath(path);
					}
					return base.LoadFromAssemblyPath(path);
				}
			}
			return null;
		}

		public Assembly? MyLoad(AssemblyName assemblyName) => Load(assemblyName);
	}
}
