using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.DependencyInjection;
using NacHelpers.Extensions;
using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Common.Model.Reflections;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Infrastructure
{
	class ApiFactory : IApiFactory
	{
		private readonly IOpenHostFactory factory;

		public ServiceProvider? Provider { get; set; }
		public IOpenModelFactory? ModelFactory { get; set; }
		public ReactiveProperty<IObservable<Unit>> OnProjectChangedProperty { get; }

		public ApiFactory(IOpenHostFactory factory)
		{
			this.factory = factory;
			OnProjectChangedProperty = new(Observable.Never<Unit>());
		}

		public IDataAssemblyRepository ResolveDataAssemblyRepository() => factory.ResolveDataAssemblyRepository();

		public CommandHistory ResolveCommandHistory() => factory.ResolveCommandHistory();
		public IObjectInterpreter ResolveObjectInterpreter() => ModelFactory?.ResolveObjectInterpreter() ?? throw new InvalidOperationException("Romanescoホストが初期化されていません。");
		public TService ResolveByPlugins<TService>() where TService : class
		{
			return Provider?.GetService<TService>() ?? throw new InvalidOperationException("Romanescoホストが初期化されていません。");
		}

		public IObservable<Unit> OnProjectChanged =>
			OnProjectChangedProperty.SelectMany(x => x);
	}
}
