using System;
using System.Reactive;
using Deptorygen.Annotations;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Reflections;

namespace Romanesco.Common.Model.Interfaces
{
	[Factory]
	public interface IApiFactory
	{
		IDataAssemblyRepository ResolveDataAssemblyRepository();
		CommandHistory ResolveCommandHistory();
		IObjectInterpreter ResolveObjectInterpreter();
		TService ResolveByPlugins<TService>() where TService : class;

		IObservable<Unit> OnProjectChanged { get; }
	}
}
