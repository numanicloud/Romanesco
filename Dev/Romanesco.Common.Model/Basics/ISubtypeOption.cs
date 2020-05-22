using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common.Model.Basics
{
	public interface ISubtypeOption
	{
		string OptionName { get; }
		IFieldState MakeState();
		bool IsTypeOf(Type type);
	}
}
