using Romanesco.Common.Model.Basics;

namespace Romanesco.Common.Model.Interfaces
{
	public interface IObjectInterpreter
	{
		IFieldState InterpretAsState(ValueStorage settability);
	}
}
