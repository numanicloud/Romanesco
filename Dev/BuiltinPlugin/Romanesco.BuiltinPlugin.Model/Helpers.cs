using Romanesco.Common.Model.Interfaces;

namespace Romanesco.BuiltinPlugin.Model;

internal static class Helpers
{
	public static void BreakIfNotLoading(this ILoadingStateReader loadingState)
	{
		if (!loadingState.IsLoading)
		{
#if TRACE_FACTORY
			System.Diagnostics.Debugger.Break();
#endif
		}
	}
}