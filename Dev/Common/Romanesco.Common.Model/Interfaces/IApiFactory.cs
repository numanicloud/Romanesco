using System;
using System.Reactive;
using Deptorygen.Annotations;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Reflections;
using Romanesco.Common.Model.ValueStorages;

namespace Romanesco.Common.Model.Interfaces
{
	[Factory]
	public interface IApiFactory : ILoadingStateProvider
	{
		IDataAssemblyRepository ResolveDataAssemblyRepository();
		CommandHistory ResolveCommandHistory();
		ValueClipBoard ResolveValueClipBoard();
		IObjectInterpreter ResolveObjectInterpreter();
		TService ResolveByPlugins<TService>() where TService : class;
		IStorageCloneService ResolveStorageCloneService();

		IObservable<Unit> OnProjectChanged { get; }
	}
}
