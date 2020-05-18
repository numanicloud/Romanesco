using Romanesco.BuiltinPlugin.View.DataContext;
using Romanesco.BuiltinPlugin.View.View;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Controls;

namespace Romanesco.BuiltinPlugin.View.Factories
{
    public class ArrayViewFactory : IViewFactory
    {
        public StateViewContext? InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is ListViewModel array)
            {
                var context = new ListContext(array, interpretRecursively);
                var onError = new Subject<Exception>();

                array.AddCommand.Subscribe(_ =>
                {
                    try
                    {
                        array.AddNewElement();
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
                    }
                    catch (Exception ex)
                    {
                        onError.OnNext(ContentAccessException.GetListError(ex));
                    }
                });

                context.SelectedIndex.Where(i => i < context.Elements.Count)
                    .Subscribe(index =>
                {
                    try
                    {
                        if (index >= 0)
                        {
                            context.SelectedControl.Value = context.Elements[index].BlockControl;
                        }
                        else
                        {
                            context.SelectedControl.Value = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        onError.OnNext(ContentAccessException.GetListError(ex));
                    }
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
