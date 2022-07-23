using System.Collections.Generic;
using System.Windows;

namespace Romanesco.Common.View.Interfaces;

public interface IResourceDictionaryFactory
{
	IEnumerable<ResourceDictionary> Get();
}