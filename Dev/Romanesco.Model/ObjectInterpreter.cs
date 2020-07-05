using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.Model
{
	public class ObjectInterpreter : IObjectInterpreter
	{
		public IStateFactory[] Factories { get; }

		public ObjectInterpreter(IEnumerable<IStateFactory> factories)
		{
			Factories = factories.ToArray();
		}

		public IFieldState InterpretRootAsState(object projectObject, PropertyInfo property)
		{
			return InterpretAsState(new ValueStorage(projectObject, property));
		}

		public IFieldState InterpretRootAsState(object projectObject, FieldInfo field)
		{
			return InterpretAsState(new ValueStorage(projectObject, field));
		}

		private IFieldState InterpretAsState(ValueStorage settability)
		{
			foreach (var factory in Factories)
			{
				var result = factory.InterpretAsState(settability, InterpretAsState);
				if (result != null)
				{
					return result;
				}
			}
			return new NoneState();
		}

		IFieldState IObjectInterpreter.InterpretAsState(ValueStorage settability)
		{
			return InterpretAsState(settability);
		}
	}
}
