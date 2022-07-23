using System.Windows;
using Romanesco.Common.View.Interfaces;

namespace Romanesco.BuiltinPlugin.View;

public class ResourceDictionaryFactory : IResourceDictionaryFactory
{
	public ResourceDictionary Get()
	{
		return new View2.BlockProperty();
	}
}