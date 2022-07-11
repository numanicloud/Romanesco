using System;
using System.Reactive;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.Reflections;

namespace Romanesco.BuiltinPlugin
{
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

		public ValueClipBoard ResolveValueClipBoard()
		{
			return _api.ResolveValueClipBoard();
		}

		public IObjectInterpreter ResolveObjectInterpreter()
		{
			return _api.ResolveObjectInterpreter();
		}

		public TService ResolveByPlugins<TService>() where TService : class
		{
			return _api.ResolveByPlugins<TService>();
		}

		public IStorageCloneService ResolveStorageCloneService()
		{
			return _api.ResolveStorageCloneService();
		}

		public IObservable<Unit> OnProjectChanged => _api.OnProjectChanged;

		public bool IsLoading
		{
			get => _api.IsLoading;
			set => _api.IsLoading = value;
		}
	}
}
