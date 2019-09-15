using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romanesco.Common;
using Romanesco.View.DataContext;
using Romanesco.ViewModel.States;

namespace Romanesco.View.Factories
{
    public class ClassViewFactory : Common.IViewFactory
    {
        public object InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is ClassViewModel @class)
            {
                var children = @class.Fields.Select(x => interpretRecursively(x)).ToArray();
                var block = new View.ClassView()
                {
                    DataContext = new ClassBlockContext(children),
                };
                return new Common.Utility.StateViewContext(null, block);
            }
            return null;
        }
    }
}
