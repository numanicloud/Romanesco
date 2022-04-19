using Imfact.Annotations;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.Reflections;

namespace Romanesco.BuiltinPlugin
{
	[Factory]
	internal class ApiFactory : IApiFactory
	{
		private readonly IApiFactory _api;

		public ApiFactory(IApiFactory api)
		{
			_api = api;
		}

		public IDataAssemblyRepository ResolveDataAssemblyRepository()
		{
			return _api.ResolveDataAssemblyRepository();
		}

		public CommandHistory ResolveCommandHistory()
		{
			return _api.ResolveCommandHistory();
		}

		public IObjectInterpreter ResolveObjectInterpreter()
		{
			return _api.ResolveObjectInterpreter();
		}

		public TService ResolveByPlugins<TService>() where TService : class
		{
			return _api.ResolveByPlugins<TService>();
		}
	}
}
