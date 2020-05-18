using Romanesco.BuiltinPlugin.View.View;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;

namespace Romanesco.BuiltinPlugin.View.Factories
{
    public class EnumViewFactory : IViewFactory
    {
        public StateViewContext? InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is EnumViewModel @enum)
            {
                var inline = new EnumView() { DataContext = @enum };
                var block = new EnumView() { DataContext = @enum };
                return new StateViewContext(inline, block, @enum);
            }
            return null;
        }
    }
}
