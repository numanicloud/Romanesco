using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.View.View;
using Romanesco.ViewModel.States;

namespace Romanesco.View.Factories
{
    public class PrimitiveViewFactory : IViewFactory
    {
        public StateViewContext InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
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

        private StateViewContext TextView(IStateViewModel viewModel)
        {
            var control = new TextView() { DataContext = viewModel };
            return new StateViewContext(control, control, viewModel);
        }

        private StateViewContext CheckView(IStateViewModel viewModel)
        {
            var control = new CheckboxView { DataContext = viewModel };
            return new StateViewContext(control, control, viewModel);
        }
    }
}
