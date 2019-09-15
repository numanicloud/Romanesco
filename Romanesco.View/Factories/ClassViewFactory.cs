using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.View.DataContext;
using Romanesco.ViewModel.States;

namespace Romanesco.View.Factories
{
    public class ClassViewFactory : Common.IViewFactory
    {
        public StateViewContext InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is ClassViewModel @class)
            {
                var children = @class.Fields.Select(x => interpretRecursively(x)).ToArray();
                var context = new ClassBlockContext(@class, children);
                foreach (var field in children)
                {
                    field.ViewModel.ShowDetail.Subscribe(_ => context.ClosedUpView.Value = field.BlockControl);
                }

                var blockControl = new View.ClassView()
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
