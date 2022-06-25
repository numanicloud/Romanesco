using System;
using NacHelpers.Extensions;
using Romanesco.BuiltinPlugin.View.DataContext;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.View.Factories
{
	public class SubtypingViewFactory : IViewFactory
	{
		public StateViewContext? InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
		{
			if (viewModel is SubtypingClassViewModel subtyping)
			{
				subtyping.OnOpenCommand.Subscribe(_ =>
				{
					if (subtyping.CurrentViewModel.Value is {} current)
					{
						current.OnOpenCommand.Execute();
					}
				}).AddTo(subtyping.Disposables);

				var context = new SubtypingContext(subtyping, interpretRecursively);
				var blockControl = new View.SubtypingBlockView()
				{
					DataContext = context,
				};
				var inlineControl = new View.SubtypingInlineView()
				{
					DataContext = context,
				};
				return new StateViewContext(inlineControl, blockControl, subtyping);
			}
			return null;
		}
	}
}
