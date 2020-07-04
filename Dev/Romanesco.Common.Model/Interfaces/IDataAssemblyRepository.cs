using System;
using System.Reflection;

namespace Romanesco.Common.Model.Interfaces
{
	public interface IDataAssemblyRepository
	{
		Assembly LoadAssemblyFromPath(string path);
		object? CreateInstance(Type type, params object?[]? args);
	}
}
