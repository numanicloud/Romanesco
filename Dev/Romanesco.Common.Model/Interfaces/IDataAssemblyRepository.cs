using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Romanesco.Common.Model.Interfaces
{
	public interface IDataAssemblyRepository
	{
		Assembly? LoadAssemblyFromPath(string path);
		object? CreateInstance(Type type, params object?[]? args);
	}
}
