using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.BuiltinPlugin.Model.Basics
{
	public class NullSubtypeOption : ISubtypeOption
	{
		public string OptionName => "<null>";

		public IFieldState MakeState() => new NoneState();

		public bool IsTypeOf(Type type) => false;
	}
}
