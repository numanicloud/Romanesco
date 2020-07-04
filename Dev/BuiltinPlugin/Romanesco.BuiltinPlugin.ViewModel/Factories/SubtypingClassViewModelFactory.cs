using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.Factories
{
	public class SubtypingClassViewModelFactory : IStateViewModelFactory
	{
		public IStateViewModel? InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
		{
			if (state is SubtypingClassState vm)
			{
				return new SubtypingClassViewModel(vm, interpretRecursively);
			}
			return null;
		}
	}
}
