using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.BuiltinPlugin.Model;

public static class Helpers
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

	public static IEnumerable<T> FilterNullRef<T>(this IEnumerable<T?> source)
		where T : notnull
	{
		foreach (var item in source)
		{
			if (item is not null)
			{
				yield return item;
			}
		}
	}

	public static IObservable<T> FilterNullRef<T>(this IObservable<T?> source)
		where T : notnull
	{
		return source.Where(x => x is not null)!;
	}
}