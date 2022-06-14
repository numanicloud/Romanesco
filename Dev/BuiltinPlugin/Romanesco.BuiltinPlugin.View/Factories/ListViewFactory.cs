using Romanesco.BuiltinPlugin.View.DataContext;
using Romanesco.BuiltinPlugin.View.View;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Windows.Controls;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.ViewModel.States;

namespace Romanesco.BuiltinPlugin.View.Factories
{
    public class ArrayViewFactory : IViewFactory
    {
        public StateViewContext? InterpretAsView(IStateViewModel viewModel, ViewInterpretFunc interpretRecursively)
        {
            if (viewModel is ListViewModel array)
            {
                var context = new ListContext(array, interpretRecursively, array.SelectedIndex);
                var onError = new Subject<Exception>();

                context.SelectedIndex.Where(i => i < context.Elements.Count)
                    .Subscribe(index =>
                {
                    try
                    {
						Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
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
                }).AddTo(array.Disposables);

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
