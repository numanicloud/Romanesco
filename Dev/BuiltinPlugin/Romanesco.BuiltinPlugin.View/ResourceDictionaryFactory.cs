using System;
using System.Collections.Generic;
using System.Windows;
using Romanesco.Common.View.Interfaces;

namespace Romanesco.BuiltinPlugin.View;

public class ResourceDictionaryFactory : IResourceDictionaryFactory
{
	public IEnumerable<ResourceDictionary> Get()
	{
		yield return new ResourceDictionary()
		{
			Source = new Uri("pack://application:,,,/Romanesco.BuiltinPlugin.View;component/View2/InlineProperty.xaml")
		};
		yield return new ResourceDictionary()
		{
			Source = new Uri("pack://application:,,,/Romanesco.BuiltinPlugin.View;component/View2/BlockProperty.xaml")
		};
	}
}