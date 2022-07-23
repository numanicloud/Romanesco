using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.Factories;

internal class PrimitiveListViewModelFactory : IStateViewModelFactory
{
	public IStateViewModel? InterpretAsViewModel
		(IFieldState state, ViewModelInterpretFunc interpretRecursively)
	{
		if (state is not ListState list || list.ElementType != typeof(int)) return null;

		return new PrimitiveListViewModel(list, interpretRecursively);
	}
}