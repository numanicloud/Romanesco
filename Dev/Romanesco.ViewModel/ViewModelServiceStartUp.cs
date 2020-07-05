using Microsoft.Extensions.DependencyInjection;
using Romanesco.Model.Services;
using Romanesco.ViewModel.Editor;
using Romanesco.ViewModel.Project;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel
{
	public class ViewModelServiceStartUp
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IEditorViewModel, EditorViewModel>()
				.AddSingleton<ViewModelInterpreter>()
				.AddTransient<IProjectSettingProvider, VmProjectSettingsProvider>();
		}
	}
}
