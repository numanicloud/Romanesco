using System;
using Imfact.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.BuiltinPlugin.View.Factories;
using Romanesco.BuiltinPlugin.ViewModel.Factories;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin
{
	[Factory]
	internal partial class PluginFactory
	{
		[Resolution(typeof(ApiFactory))]
		[Cache]
		public partial IApiFactory ResolveApiFactory();

		// Models
		[Resolution(typeof(DynamicStateFactory))]
		[Cache]
		public partial IStateFactory ResolveDynamicStateFactory();

		[Resolution(typeof(IdStateFactory))]
		[Cache]
		public partial IStateFactory ResolveIdStateFactory();

		[Resolution(typeof(ListStateFactory))]
		[Cache]
		public partial IStateFactory ResolveListStateFactory();

		[Resolution(typeof(PrimitiveStateFactory))]
		[Cache]
		public partial PrimitiveStateFactory ResolvePrimitiveStateFactory();

		[Resolution(typeof(PrimitiveStateFactory))]
		[Cache]
		public partial IStateFactory ResolvePrimitiveStateFactory2();

		[Resolution(typeof(EnumStateFactory))]
		[Cache]
		public partial IStateFactory ResolveEnumStateFactory();

		[Resolution(typeof(SubtypingStateFactory))]
		[Cache]
		public partial IStateFactory ResolveSubtypingStateFactory();

		[Resolution(typeof(ClassStateFactory))]
		[Cache]
		public partial IStateFactory ResolveClassStateFactory();

		// ViewModels
		[Resolution(typeof(IdViewModelFactory))]
		[Cache]
		public partial IStateViewModelFactory ResolveIdViewModelFactory();

		[Resolution(typeof(PrimitiveViewModelFactory))]
		[Cache]
		public partial IStateViewModelFactory ResolvePrimitiveViewModelFactory();

		[Resolution(typeof(EnumViewModelFactory))]
		[Cache]
		public partial IStateViewModelFactory ResolveEnumViewModelFactory();

		[Resolution(typeof(SubtypingClassViewModelFactory))]
		[Cache]
		public partial IStateViewModelFactory ResolveSubtypingClassViewModelFactory();

		[Resolution(typeof(ClassViewModelFactory))]
		[Cache]
		public partial IStateViewModelFactory ResolveClassViewModelFactory();

		[Resolution(typeof(ListViewModelFactory))]
		[Cache]
		public partial IStateViewModelFactory ResolveListViewModelFactory();

		// View
		[Resolution(typeof(IdViewFactory))]
		[Cache]
		public partial IViewFactory ResolveIdViewFactory();

		[Resolution(typeof(PrimitiveViewFactory))]
		[Cache]
		public partial IViewFactory ResolvePrimitiveViewFactory();

		[Resolution(typeof(EnumViewFactory))]
		[Cache]
		public partial IViewFactory ResolveEnumViewFactory();

		[Resolution(typeof(ClassViewFactory))]
		[Cache]
		public partial IViewFactory ResolveClassViewFactory();

		[Resolution(typeof(ArrayViewFactory))]
		[Cache]
		public partial IViewFactory ResolveArrayViewFactory();

		[Resolution(typeof(SubtypingViewFactory))]
		[Cache]
		public partial IViewFactory ResolveSubtypingViewFactory();

		[Resolution(typeof(IdListViewFactory))]
		[Cache]
		public partial IViewFactory ResolveIntIdChoiceListViewFactory();

		[Exporter]
		public void ConfigureServices<TInterface>
			(IServiceCollection services, Func<TInterface> func)
			where TInterface : class
		{
			services.AddTransient(_ => func());
		}
	}
}
