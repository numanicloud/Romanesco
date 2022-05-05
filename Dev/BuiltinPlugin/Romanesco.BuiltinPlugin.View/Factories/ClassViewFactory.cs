using Romanesco.BuiltinPlugin.View.DataContext;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Linq;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.ViewModel.States;

namespace Romanesco.BuiltinPlugin.View.Factories
{
	public class ClassViewFactory : IViewFactory
	{
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
				field.ViewModel.ShowDetail.Subscribe(_ => context.ClosedUpView.Value = field.BlockControl)
					.AddTo(@class.Disposables);
			}

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
