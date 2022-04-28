using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.View.States
{
	internal class ViewInterpreter
    {
        private readonly IViewFactory[] factories;

        public ViewInterpreter(IEnumerable<IViewFactory> factories)
        {
            this.factories = factories.ToArray();
        }

		private static int Depth = 0;

        public StateViewContext InterpretAsView(IStateViewModel viewModel)
		{
			using var scope = new HandlingDisposable(() =>
			{
				//Debug.WriteLine($"Out: {string.Join("", Enumerable.Repeat("\t", Depth))}, {viewModel.Title.Value}, {viewModel.GetType()}", "Romanesco");
            });
			//Debug.WriteLine($" In: {string.Join("", Enumerable.Repeat("\t", Depth))}, {viewModel.Title.Value}, {viewModel.GetType()}", "Romanesco");

            Depth++;
            foreach (var factory in factories)
            {
                var result = factory.InterpretAsView(viewModel, InterpretAsView);
                if (result != null)
                {
					Depth--;
                    return result;
                }
            }

			Depth--;
            return new StateViewContext(new NoneView(), new NoneView(), new NoneViewModel(new NoneState()));
        }
    }
}
