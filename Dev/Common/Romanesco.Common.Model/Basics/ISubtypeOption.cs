using Romanesco.Common.Model.Interfaces;
using System;

namespace Romanesco.Common.Model.Basics
{
	public interface ISubtypeOption
	{
		string OptionName { get; }
		IFieldState MakeState(ValueStorage valueStorage);
		bool IsTypeOf(Type type);
	}
}
