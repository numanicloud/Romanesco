using Romanesco.BuiltinPlugin.View.View;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;

namespace Romanesco.BuiltinPlugin.View.Factories
{
    public class PrimitiveViewFactory : IViewFactory
    {
        public StateViewContext? InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            return viewModel switch
            {
                IntViewModel i => TextView(i),
                ByteViewModel b => TextView(b),
                ShortViewModel s => TextView(s),
                LongViewModel l => TextView(l),
                FloatViewModel f => TextView(f),
                DoubleViewModel d => TextView(d),
                StringViewModel s => TextView(s),
                BoolViewModel b => CheckView(b),
                _ => null,
            };
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
