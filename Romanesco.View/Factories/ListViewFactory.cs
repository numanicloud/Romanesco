using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.View.DataContext;
using Romanesco.View.View;
using Romanesco.ViewModel.States;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace Romanesco.View.Factories
{
    public class ArrayViewFactory : IViewFactory
    {
        public StateViewContext InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is ListViewModel array)
            {
                var context = new ArrayContext(array);
                array.AddCommand.Subscribe(_ =>
                {
                    var vm = array.AddNewElement();
                    var view = interpretRecursively(vm);
                    context.Elements.Add(view);
                });
                array.RemoveCommand.Where(i => i < array.Elements.Count)
                    .Subscribe(index =>
                {
                    array.RemoveAt(index);
                    array.Elements.RemoveAt(index);
                });
                context.SelectedIndex.Where(i => i < array.Elements.Count)
                    .Subscribe(index =>
                {
                    context.SelectedControl.Value = context.Elements[index].BlockControl;
                });

                var blockControl = new ListBlockView()
                {
                    DataContext = context,
                };
                var inlineControl = new ListInlineView()
                {
                    DataContext = context,
                };

                return new StateViewContext(inlineControl, blockControl, array);
            }
            return null;
        }
    }
}
