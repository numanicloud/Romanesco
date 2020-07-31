using System;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.Reflections;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Infrastructure
{
	class ApiFactory : IApiFactory
	{
		private readonly IOpenHostFactory factory;
		public IOpenModelFactory? ModelFactory { get; set; }

		public ApiFactory(IOpenHostFactory factory)
		{
			this.factory = factory;
		}

		public IDataAssemblyRepository ResolveDataAssemblyRepository() => factory.ResolveDataAssemblyRepository();

		public CommandHistory ResolveCommandHistory() => factory.ResolveCommandHistory();
		public IObjectInterpreter ResolveObjectInterpreter() => ModelFactory?.ResolveObjectInterpreter() ?? throw new InvalidOperationException("ホストが初期化されていません。");
	}
}
