using Deptorygen.Annotations;
using Deptorygen.GenericHost;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Reflections;
using Romanesco.Extensibility;
using Romanesco.Model;
using Romanesco.Startup;

namespace Romanesco.Infrastructure
{
	[Factory]
	[ConfigureGenericHost]
	internal interface IHostFactory : IOpenHostFactory
	{
		PluginLoader ResolvePluginLoaderAsTransient();
		MainWindow ResolveMainWindow();
	}

	[Factory]
	public interface IOpenHostFactory
	{
		[Resolution(typeof(DataAssemblyRepository))]
		IDataAssemblyRepository ResolveDataAssemblyRepository();
		CommandHistory ResolveCommandHistory();
	}
}
