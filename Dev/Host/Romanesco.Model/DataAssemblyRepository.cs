using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using NacHelpers.Extensions;
using NacHelpers.FilePath;
using NacHelpers.FilePath.Interfaces;
using Romanesco.Common.Model.Reflections;

namespace Romanesco.Model
{
	public class DataAssemblyRepository : IDataAssemblyRepository
	{
		private readonly List<AbsoluteDirectoryPath> probingDirectories = new List<AbsoluteDirectoryPath>();

		public object? CreateInstance(Type type, params object?[]? args)
		{
			if (type.Assembly.ReflectionOnly)
			{
				return new DynamicMock(type.GetProperties(), type.GetFields());
			}
			else
			{
				return Activator.CreateInstance(type, args);
			}
		}

		public Assembly LoadAssemblyFromPath(string path)
		{
			var fullPath = path.AsRelativeFilePath().ToAbsoluteFilePath();
			var assemblyName = AssemblyName.GetAssemblyName(path);
			if (assemblyName.Name is { } name)
			{
				if (assemblyName.ProcessorArchitecture == GetType().Assembly.GetName().ProcessorArchitecture)
				{
					return LoadFullAssembly(fullPath);
				}
				else
				{
					var resolver = GetResolver(fullPath);
					var loader = new MetadataLoadContext(resolver, typeof(object).Assembly.GetName().Name);
					return loader.LoadFromAssemblyPath(fullPath.ToStringRepresentation());
				}
			}

			throw new InvalidOperationException("AssemblyName が無効です");
		}

		private Assembly LoadFullAssembly(AbsoluteFilePath fullPath)
		{
			var loadContext = AssemblyLoadContext.Default;

			var dirPath = fullPath.GetParentAbsoluteDirectoryPath();
			if (!probingDirectories.Contains(dirPath))
			{
				probingDirectories.Add(dirPath);
			}

			loadContext.Resolving += DefaultOnResolving;
			var assembly = loadContext.LoadFromAssemblyPath(fullPath.ToStringRepresentation());
			var dependencies = LoadDependencies(loadContext, assembly).ToLinear().ToArray();
			loadContext.Resolving -= DefaultOnResolving;
			return assembly;
		}

		private IEnumerable<IEnumerable<Assembly>> LoadDependencies(AssemblyLoadContext context, Assembly root)
		{
			yield return new[] { root };
			foreach (var dependency in root.GetReferencedAssemblies())
			{
				if (dependency.Name is null)
				{
					continue;
				}

				var path = probingDirectories.SelectMany(x => x.EnumerateFile())
					.FirstOrDefault(x => x.WithoutExtension().GetFileName() == dependency.Name);

				if (path is {})
				{
					var assembly = context.LoadFromAssemblyPath(path.ToStringRepresentation());
					yield return LoadDependencies(context, assembly).ToLinear();
				}
			}
		}

		private Assembly? DefaultOnResolving(AssemblyLoadContext arg1, AssemblyName arg2)
		{
			Debug.WriteLine($"DataAssemblyRepository: {arg2} の読み込みが試行されました");
			var path = probingDirectories.SelectMany(x => x.EnumerateFile())
				.FirstOrDefault(x => x.WithoutExtension().GetFileName() == arg2.Name);

			if (path is null)
			{
				return null;
			}
			return arg1.LoadFromAssemblyPath(path.ToStringRepresentation());
		}

		private MetadataAssemblyResolver GetResolver(IAbsolutePath entryAssemblyPath)
		{
			var entryDirAsms = entryAssemblyPath.GetParentAbsoluteDirectoryPath()
				.EnumerateFile()
				.Where(x => x.GetExtension() == ".dll");

			var runtimeDirAsms = RuntimeEnvironment.GetRuntimeDirectory()
				.AsAbsoluteDirectoryPath()
				.EnumerateFile()
				.Where(x => x.GetExtension() == ".dll");

			return new PathAssemblyResolver(entryDirAsms.Concat(runtimeDirAsms)
				.Select(x => x.ToStringRepresentation()));
		}
	}
}
