using Romanesco.BuiltinPlugin.View.DataContext;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.ViewModel.States;

namespace Romanesco.BuiltinPlugin.View.Factories
{
	public class ClassViewFactory : IViewFactory
	{
		private readonly Dictionary<Type, string> _typeToFocusedTitle = new();

		public StateViewContext? InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
		{
			if (viewModel is not ClassViewModel @class)
			{
				return null;
			}

			var children = @class.Fields.Select(x => interpretRecursively(x)).ToArray();
			var context = new ClassContext(@class, children);
			foreach (var field in children)
			{
				field.ViewModel.ShowDetail.Subscribe(_ =>
					{
						context.ClosedUpView.Value = field.BlockControl;
						_typeToFocusedTitle[@class.State.Type] = field.ViewModel.Title.Value;
					})
					.AddTo(@class.Disposables);
			}

			@class.OnOpenCommand.Subscribe(_ =>
			{
				if (_typeToFocusedTitle.ContainsKey(@class.State.Type))
				{
					var child = children
						.First(x => x.ViewModel.Title.Value == _typeToFocusedTitle[@class.State.Type]);
					context.ClosedUpView.Value = child.BlockControl;
					if (child.ViewModel is IOpenCommandConsumer classViewModel)
					{
						classViewModel.OnOpenCommand.Execute();
					}
				}
			}).AddTo(@class.Disposables);

			var blockControl = new View.ClassBlockView()
			{
				DataContext = context,
			};
			var inlineControl = new View.ClassInlineView()
			{
				DataContext = context,
			};
			return new StateViewContext(inlineControl, blockControl, @class);
		}
	}
}
