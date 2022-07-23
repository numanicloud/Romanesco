using System;
using System.Reactive;
using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.Utilities;

public class DetailedViewModel<TInline, TBlock> : IStateViewModel
	where TInline : IStateViewModel
	where TBlock : IStateViewModel
{
	public IReadOnlyReactiveProperty<string> Title => InlineViewModel.Title;
	public IReadOnlyReactiveProperty<string> FormattedString => InlineViewModel.FormattedString;
	public IObservable<Unit> ShowDetail => InlineViewModel.ShowDetail;
	public IObservable<Exception> OnError => InlineViewModel.OnError;

	public TInline InlineViewModel { get; }
	public TBlock BlockViewModel { get; }

	public DetailedViewModel(TInline inlineViewModel, TBlock blockViewModel)
	{
		InlineViewModel = inlineViewModel;
		BlockViewModel = blockViewModel;
	}
}

internal static class DetailedViewModel
{
	public static DetailedViewModel<TInline, TBlock> Create<TInline, TBlock>
		(TInline inline, TBlock block)
		where TInline : IStateViewModel
		where TBlock : IStateViewModel
	{
		return new DetailedViewModel<TInline, TBlock>(inline, block);
	}
}