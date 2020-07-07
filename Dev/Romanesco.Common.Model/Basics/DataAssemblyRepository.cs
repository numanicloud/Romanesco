using Romanesco.Common.Model.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace Romanesco.Common.Model.Basics
{
	public class DataAssemblyRepository : IDataAssemblyRepository
	{
		private bool isInitialized = false;

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
					var resolver = GetResolver(fullPath);
					var loader = new MetadataLoadContext(resolver, typeof(object).Assembly.GetName().Name);
					return loader.LoadFromAssemblyPath(fullPath);
				}
			}
			else
			{
				throw new InvalidOperationException("AssemblyName が無効です");
			}
		}

		private MetadataAssemblyResolver GetResolver(string entryAssemblyPath)
		{
			var fileName = Path.GetFileName(entryAssemblyPath);
			var entryAssemblyDir = entryAssemblyPath.Replace(fileName, "");
			var romanescoDir = Environment.CurrentDirectory;
			var runtimeDir = RuntimeEnvironment.GetRuntimeDirectory();

			var annotationAsm = Path.Combine(romanescoDir, "Romanesco.Annotations.dll");
			var entryDirAsms = Directory.EnumerateFiles(entryAssemblyDir, "*.dll");
			var runtimeDirAsms = Directory.GetFiles(runtimeDir, "*.dll");

			return new PathAssemblyResolver(entryDirAsms.Concat(runtimeDirAsms));
		}
	}
}
