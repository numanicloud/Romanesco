using Romanesco.BuiltinPlugin.View.DataContext;
using Romanesco.BuiltinPlugin.View.View;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Windows.Controls;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.ViewModel.States;

namespace Romanesco.BuiltinPlugin.View.Factories;

public class ArrayViewFactory : IViewFactory
{
	public StateViewContext? InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
	{
		if (viewModel is ListViewModel array)
		{
			var context = new ListContext(array, interpretRecursively, array.SelectedIndex);
			var onError = new Subject<Exception>();

			context.SelectedIndex.Where(i => i < context.Elements.Count)
				.Subscribe(index => OnSelectedIndexChanged(index, context, array, onError)).AddTo(array.Disposables);

			var blockControl = GetBlockControl(array.ElementType, context);
			var inlineControl = new ListInlineView()
			{
				DataContext = context,
			};

			var view = new StateViewContext(inlineControl, blockControl, array);
			view.OnError = array.OnError.Merge(onError);
			return view;
		}
		return null;
	}

	private void OnSelectedIndexChanged
		(int index, ListContext context, ListViewModel array, Subject<Exception> onError)
	{
		try
		{
			if (index >= 0)
			{
				var element = context.Elements[index];
				context.SelectedControl.Value = element.BlockControl;
				if (element.ViewModel is IOpenCommandConsumer classViewModel)
				{
					classViewModel.OnOpenCommand.Execute();
				}
			}
			else
			{
				context.SelectedControl.Value = null;
			}
		}
		catch (Exception ex)
		{
			onError.OnNext(ContentAccessException.GetListError(ex));
		}
	}

	private UserControl GetBlockControl(Type elementType, object dataContext)
	{
		if (elementType == typeof(int))
		{
			return new PrimitiveListBlockView()
			{
				DataContext = dataContext
			};
		}
		else
		{
			return new ListBlockView()
			{
				DataContext = dataContext
			};
		}
	}
}