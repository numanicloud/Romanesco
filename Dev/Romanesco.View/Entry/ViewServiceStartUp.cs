using Microsoft.Extensions.DependencyInjection;
using Romanesco.View.States;

namespace Romanesco.View.Entry
{
	public class ViewServiceStartUp
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IEditorViewContext, MainDataContext>();
			services.AddSingleton<ViewInterpreter>();
		}
	}
}
