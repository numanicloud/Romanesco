using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.Factories;

public class EnumViewModelFactory : IStateViewModelFactory
{
	public IStateViewModel? InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
	{
		if (state is EnumState @enum)
		{
			return new EnumViewModel(@enum);
		}
		return null;
	}
}