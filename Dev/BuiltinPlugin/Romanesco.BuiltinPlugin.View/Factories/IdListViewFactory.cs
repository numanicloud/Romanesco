﻿using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.View.Factories
{
	public class IdListViewFactory : IViewFactory
	{
		public StateViewContext? InterpretAsView
			(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
		{
			if (viewModel is not IntIdChoiceListViewModel vm)
			{
				return null;
			}

			var onError = new Subject<Exception>();

			vm.AddCommand.Subscribe(_ =>
			{
				try
				{
					vm.AddNewElement();
				}
				catch (Exception ex)
				{
					onError.OnNext(ContentAccessException.GetListError(ex));
				}
			}).AddTo(vm.Disposables);

			vm.RemoveCommand.Subscribe(i =>
			{
				try
				{
					var index = vm.Elements.IndexOf(i);
					if (index != -1)
					{
						vm.RemoveAt(index);
					}
				}
				catch (Exception ex)
				{
					onError.OnNext(ContentAccessException.GetListError(ex));
				}
			}).AddTo(vm.Disposables);

			var inline = new View.IdChoiceListInlineView()
			{
				DataContext = vm
			};

			var block = new View.IdChoiceListView()
			{
				DataContext = vm
			};

			var view = new StateViewContext(inline, block, vm);
			view.OnError = vm.OnError.Merge(onError);
			return view;

		}
	}
}
