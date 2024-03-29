﻿using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.Factories;

public class PrimitiveViewModelFactory : IStateViewModelFactory
{
	public IStateViewModel? InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
	{
		return state switch
		{
			IntState i => new IntViewModel(i),
			FloatState f => new FloatViewModel(f),
			StringState s => new StringViewModel(s),
			BoolState b => new BoolViewModel(b),
			ByteState b => new ByteViewModel(b),
			ShortState s => new ShortViewModel(s),
			LongState l => new LongViewModel(l),
			DoubleState d => new DoubleViewModel(d),
			_ => null,
		};
	}
}