using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Romanesco.Common.Model.Basics
{
	public class DataAssemblyRepository : IDataAssemblyRepository
	{
		public object? CreateInstance(Type type, params object?[]? args)
		{
			if (type.Assembly.ReflectionOnly)
			{
				// TODO: DynamicObjectで生成する
				return null;
			}
			else
			{
				return Activator.CreateInstance(type, args);
			}
		}

		public Assembly LoadAssemblyFromPath(string path)
		{
			var assemblyName = AssemblyName.GetAssemblyName(path);
			if (assemblyName.Name is string name)
			{
				if (assemblyName.ProcessorArchitecture == ProcessorArchitecture.MSIL)
				{
					return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
				}
				else
				{
					var fullPath = Path.GetFullPath(path);
					var resolver = new PathAssemblyResolver(new[] { fullPath, typeof(object).Assembly.Location });
					var loader = new MetadataLoadContext(resolver, name);
					return loader.LoadFromAssemblyPath(fullPath);
				}
			}
			else
			{
				throw new InvalidOperationException("AssemblyName が無効です");
			}
		}
	}
}
