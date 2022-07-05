using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.BuiltinPlugin.Model.Basics
{
	public class NullSubtypeOption : ISubtypeOption
	{
		private readonly ValueStorage _storage;

		public NullSubtypeOption(ValueStorage storage)
		{
			_storage = storage;
		}

		public string OptionName => "<null>";

		public IFieldState MakeState(ValueStorage valueStorage)
		{
			valueStorage.SetValue(null);
			return new ClassState(valueStorage, Array.Empty<IFieldState>());
		}

		public IFieldState MakeFromStorage(ValueStorage valueStorage, ValueStorage pasteFrom)
		{
			return new NoneState();
		}

		public bool IsTypeOf(Type type) => false;
	}
}
