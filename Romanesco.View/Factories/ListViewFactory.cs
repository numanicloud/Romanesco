using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.View.DataContext;
using Romanesco.View.View;
using Romanesco.ViewModel.States;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.View.Factories
{
    public class ArrayViewFactory : IViewFactory
    {
        public StateViewContext InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is ListViewModel array)
            {
                var context = new ListContext(array);
                var onError = new Subject<Exception>();

                array.AddCommand.Subscribe(_ =>
                {
                    try
                    {
                        var vm = array.AddNewElement();
                        var view = interpretRecursively(vm);
                        context.Elements.Add(view);
                    }
                    catch (Exception ex)
                    {
                        onError.OnNext(ContentAccessException.GetListError(ex));
                    }
                });

                array.RemoveCommand.Where(i => i < array.Elements.Count)
                    .Subscribe(index =>
                {
                    try
                    {
                        array.RemoveAt(index);
                        array.Elements.RemoveAt(index);
                    }
                    catch (Exception ex)
                    {
                        onError.OnNext(ContentAccessException.GetListError(ex));
                    }
                });

                context.SelectedIndex.Where(i => i < array.Elements.Count)
                    .Subscribe(index =>
                {
                    context.SelectedControl.Value = context.Elements[index].BlockControl;
                });

                var blockControl = GetBlockControl(array.ElementType, context);
                var inlineControl = new ListInlineView()
                {
                    DataContext = context,
                };

                var view = new StateViewContext(inlineControl, blockControl, array);
                view.OnError = array.OnError.Merge(onError);
                return view;
            }
            return null;
        }

        private UserControl GetBlockControl(Type elementType, object dataContext)
        {
            if (elementType == typeof(int))
            {
                return new PrimitiveListBlockView()
                {
                    DataContext = dataContext
                };
            }
            else
            {
                return new ListBlockView()
                {
                    DataContext = dataContext
                };
            }
        }
    }
}
