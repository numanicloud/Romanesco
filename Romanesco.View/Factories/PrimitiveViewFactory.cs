using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.View.View;
using Romanesco.ViewModel.States;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.View.Factories
{
    public class PrimitiveViewFactory : IViewFactory
    {
        public object InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            switch (viewModel)
            {
            case IntViewModel i: return TextView(i);
            case ByteViewModel b: return TextView(b);
            case ShortViewModel s: return TextView(s);
            case LongViewModel l: return TextView(l);
            case FloatViewModel f: return TextView(f);
            case DoubleViewModel d: return TextView(d);
            case StringViewModel s: return TextView(s);
            case BoolViewModel b: return CheckView(b);
            default: return null;
            }
        }

        private object TextView(IStateViewModel viewModel)
        {
            var control = new TextView() { DataContext = viewModel };
            return new StateViewContext(control, control);
        }

        private object CheckView(IStateViewModel viewModel)
        {
            var control = new CheckboxView { DataContext = viewModel };
            return new StateViewContext(control, control);
        }
    }
}
