using Romanesco.Common.Model.Interfaces;
using System;

namespace Romanesco.Common.Model.Basics
{
	public interface ISubtypeOption
	{
		string OptionName { get; }
		IFieldState MakeState(ValueStorage valueStorage);
		IFieldState MakeFromStorage(ValueStorage valueStorage, ValueStorage pasteFrom);
		bool IsTypeOf(Type type);
	}
}
