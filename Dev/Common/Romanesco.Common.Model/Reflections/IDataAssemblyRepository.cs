using System;
using System.Reflection;

namespace Romanesco.Common.Model.Reflections
{
	public interface IDataAssemblyRepository
	{
		Assembly LoadAssemblyFromPath(string path);
		object? CreateInstance(Type type, params object?[]? args);
	}
}
