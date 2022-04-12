using Romanesco.Common.Model.Interfaces;
using System;

namespace Romanesco.Common.Model.Basics
{
	public interface ISubtypeOption
	{
		string OptionName { get; }
		IFieldState MakeState();
		bool IsTypeOf(Type type);
	}
}
