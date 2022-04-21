using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.ViewModel.States
{
	internal class ViewModelInterpreter : IViewModelInterpreter
    {
        private readonly IStateViewModelFactory[] factories;

        public ViewModelInterpreter(IEnumerable<IStateViewModelFactory> factories)
        {
            this.factories = factories.ToArray();
        }

        private int Depth { get; set; }
        public IStateViewModel InterpretAsViewModel(IFieldState state)
		{
            /*
			using var scope = new HandlingDisposable(() =>
			{
				Debug.WriteLine($"Out: {string.Join("", Enumerable.Repeat("\t", Depth))} {state.Title.Value}, {state.GetType()}", "Romanesco");
            });
			Debug.WriteLine($" In: {string.Join("", Enumerable.Repeat("\t", Depth))} {state.Title.Value}, {state.GetType()}", "Romanesco");
            */

            Depth++;
            foreach (var factory in factories)
            {
                var result = factory.InterpretAsViewModel(state, InterpretAsViewModel);
                if (result != null)
				{
                    Depth--;
                    return result;
                }
            }

			Depth--;
            return new NoneViewModel(new NoneState());
        }
    }
}
