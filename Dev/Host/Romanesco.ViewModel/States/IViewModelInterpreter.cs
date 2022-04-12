using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.ViewModel.States
{
	public interface IViewModelInterpreter
	{
		IStateViewModel InterpretAsViewModel(IFieldState state);
	}
}
