using Microsoft.Extensions.DependencyInjection;
using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Extensibility.Interfaces;
using Romanesco.Common.Extensibility;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.Factories;
using Romanesco.BuiltinPlugin.ViewModel.Factories;
using Romanesco.BuiltinPlugin.View.Factories;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.View.Interfaces;

namespace Romanesco.BuiltinPlugin
{
	public class PluginService : IPluginService
	{
		public void ConfigureServices(IServiceCollection services)
		{
			var masterListContext = new MasterListContext();

			// Model
			services.AddSingleton<IStateFactory>(provider => new IdStateFactory(masterListContext))
				.AddSingleton<IStateFactory>(provider => new ListStateFactory(masterListContext, provider.GetRequiredService<CommandHistory>()))
				.AddSingletons<IStateFactory>(typeof(PrimitiveStateFactory),
					typeof(EnumStateFactory),
					typeof(SubtypingStateFactory),
					typeof(ClassStateFactory));

			// ViewModel
			services.AddSingletons<IStateViewModelFactory>(typeof(IdViewModelFactory),
				typeof(PrimitiveViewModelFactory),
				typeof(EnumViewModelFactory),
				typeof(SubtypingClassViewModelFactory),
				typeof(ClassViewModelFactory),
				typeof(ListViewModelFactory));

			// View
			services.AddSingletons<IViewFactory>(typeof(IdViewFactory),
				typeof(PrimitiveViewFactory),
				typeof(EnumViewFactory),
				typeof(ClassViewFactory),
				typeof(ArrayViewFactory),
				typeof(SubtypingViewFactory));
		}
	}
}
