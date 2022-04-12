using System.Collections.Generic;
using Deptorygen.Annotations;
using Deptorygen.GenericHost;
using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.BuiltinPlugin.View.Factories;
using Romanesco.BuiltinPlugin.ViewModel.Factories;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin
{
	[Factory]
	[ConfigureGenericHost]
	internal interface IFactory
	{
		IApiFactory Host { get; }

		// Models
		DynamicStateFactory ResolveDynamicStateFactory();
		IdStateFactory ResolveIdStateFactory();
		ListStateFactory ResolveListStateFactory();
		PrimitiveStateFactory ResolvePrimitiveStateFactory();
		EnumStateFactory ResolveEnumStateFactory();
		SubtypingStateFactory ResolveSubtypingStateFactory();
		ClassStateFactory ResolveClassStateFactory();
		[Resolution(typeof(DynamicStateFactory))]
		[Resolution(typeof(IdStateFactory))]
		[Resolution(typeof(ListStateFactory))]
		[Resolution(typeof(PrimitiveStateFactory))]
		[Resolution(typeof(EnumStateFactory))]
		[Resolution(typeof(SubtypingStateFactory))]
		[Resolution(typeof(ClassStateFactory))]
		IEnumerable<IStateFactory> ResolveStateFactories();

		// ViewModels
		IdViewModelFactory ResolveIdViewModelFactory();
		PrimitiveViewModelFactory ResolvePrimitiveViewModelFactory();
		EnumViewModelFactory ResolveEnumViewModelFactory();
		SubtypingClassViewModelFactory ResolveSubtypingClassViewModelFactory();
		ClassViewModelFactory ResolveClassViewModelFactory();
		ListViewModelFactory ResolveListViewModelFactory();
		[Resolution(typeof(IdViewModelFactory))]
		[Resolution(typeof(PrimitiveViewModelFactory))]
		[Resolution(typeof(EnumViewModelFactory))]
		[Resolution(typeof(SubtypingClassViewModelFactory))]
		[Resolution(typeof(ClassViewModelFactory))]
		[Resolution(typeof(ListViewModelFactory))]
		IEnumerable<IStateViewModelFactory> ResolveViewModelFactories();

		// View
		IdViewFactory ResolveIdViewFactory();
		PrimitiveViewFactory ResolvePrimitiveViewFactory();
		EnumViewFactory ResolveEnumViewFactory();
		ClassViewFactory ResolveClassViewFactory();
		ArrayViewFactory ResolveArrayViewFactory();
		SubtypingViewFactory ResolveSubtypingViewFactory();
		[Resolution(typeof(IdViewFactory))]
		[Resolution(typeof(PrimitiveViewFactory))]
		[Resolution(typeof(EnumViewFactory))]
		[Resolution(typeof(ClassViewFactory))]
		[Resolution(typeof(ArrayViewFactory))]
		[Resolution(typeof(SubtypingViewFactory))]
		IEnumerable<IViewFactory> ResolveViewFactories();
	}
}
