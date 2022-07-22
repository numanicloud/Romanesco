using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.Factories;

public class IdViewModelFactory : IStateViewModelFactory
{
	public IStateViewModel? InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
	{
		if (state is IntIdChoiceState intid)
		{
			return new States.IntIdChoiceViewModel(intid);
		}
		else if (state is IntIdChoiceListState list)
		{
			return new States.IntIdChoiceListViewModel(list);
		}
		return null;
	}
}