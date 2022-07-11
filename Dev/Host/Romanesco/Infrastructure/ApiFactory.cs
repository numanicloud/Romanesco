using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.Reflections;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Infrastructure
{
	class ApiFactory : IApiFactory
	{
		private static int NextId = 0;

		private readonly IOpenHostFactory factory;
		private readonly int Id;

		public ServiceProvider? Provider { get; set; }
		public IOpenModelFactory? ModelFactory { get; set; }
		public ReactiveProperty<IObservable<Unit>> OnProjectChangedProperty { get; }

		public ApiFactory(IOpenHostFactory factory)
		{
			this.factory = factory;
			OnProjectChangedProperty = new(Observable.Never<Unit>());
			Id = NextId++;
		}

		public IDataAssemblyRepository ResolveDataAssemblyRepository() => factory.ResolveDataAssemblyRepository();

		public ValueClipBoard ResolveValueClipBoard() => factory.ResolveValueClipBoard();
		public CommandHistory ResolveCommandHistory() => factory.ResolveCommandHistory();
		public IObjectInterpreter ResolveObjectInterpreter() => ModelFactory?.ResolveObjectInterpreter() ?? throw new InvalidOperationException("Romanescoホストが初期化されていません。");
		public TService ResolveByPlugins<TService>() where TService : class
		{
			return Provider?.GetService<TService>() ?? throw new InvalidOperationException("Romanescoホストが初期化されていません。");
		}

		public IStorageCloneService ResolveStorageCloneService() =>
			ModelFactory?.ResolveStorageCloneService() ?? throw new InvalidOperationException();

		public IObservable<Unit> OnProjectChanged =>
			OnProjectChangedProperty.SelectMany(x => x);

		public bool IsLoading { get; set; }

		public override int GetHashCode()
		{
			return Id;
		}
	}
}
