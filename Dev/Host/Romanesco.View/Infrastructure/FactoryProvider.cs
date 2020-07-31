namespace Romanesco.View.Infrastructure
{
	public static class FactoryProvider
	{
		private static IOpenViewFactory? _openViewFactory;

		public static IOpenViewFactory GetOpenViewFactory(IViewRequirementFactory requirement, IPluginFactory plugin)
		{
			return _openViewFactory ??= new ViewFactory(requirement, plugin);
		}
	}
}
