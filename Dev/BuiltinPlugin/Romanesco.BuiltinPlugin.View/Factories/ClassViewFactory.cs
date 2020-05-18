using Romanesco.BuiltinPlugin.View.DataContext;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;
using System;
using System.Linq;

namespace Romanesco.BuiltinPlugin.View.Factories
{
    public class ClassViewFactory : IViewFactory
    {
        public StateViewContext? InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is ClassViewModel @class)
            {
                var children = @class.Fields.Select(x => interpretRecursively(x)).ToArray();
                var context = new ClassContext(@class, children);
                foreach (var field in children)
                {
                    field.ViewModel.ShowDetail.Subscribe(_ => context.ClosedUpView.Value = field.BlockControl);
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
            return null;
        }
    }
}
