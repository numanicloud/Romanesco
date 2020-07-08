using System;
using System.Linq;
using System.Reflection;
using NacHelpers.Extensions;
using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.Reflections;

namespace Romanesco.BuiltinPlugin.Model.Factories
{
	public class DynamicStateFactory : IStateFactory
	{
		private readonly PrimitiveStateFactory primitiveStateFactory;

		private static readonly (Type type, object? @default)[] PrimitiveTypes = {
			(typeof(int), default(int)),
			(typeof(float), default(float)),
			(typeof(string), string.Empty),
			(typeof(bool), default(bool)),
			(typeof(byte), default(byte)),
			(typeof(short), default(short)),
			(typeof(long), default(long)),
			(typeof(double), default(double)),
		};

		public DynamicStateFactory(PrimitiveStateFactory primitiveStateFactory)
		{
			this.primitiveStateFactory = primitiveStateFactory;
		}

		public IFieldState? InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
		{
			var type = settability.Type;
			if (!type.Assembly.ReflectionOnly)
			{
				return null;
			}

			if (TryGetPrimitiveStorage(type, settability) is {} primitive)
			{
				return primitiveStateFactory.InterpretAsState(primitive, interpret);
			}

			var storageFactory = new ValueStorageFactory();
			DynamicMock mock = GetOrCreateMock(settability);

			var storages = from key in mock.Keys
						   where mock.GetAttributeData<EditorMemberAttribute>(key) != null
						   select storageFactory.FromDynamicMocksMember(mock, key) into storage
						   select interpret(storage);
			return new ClassState(settability, storages.FilterNullRef().ToArray());
		}

		private ValueStorage? TryGetPrimitiveStorage(Type roType, ValueStorage source)
		{
			for (int i = 0; i < PrimitiveTypes.Length; i++)
			{
				if (PrimitiveTypes[i].type.FullName == roType.FullName)
				{
					return new ValueStorage(
						PrimitiveTypes[i].type,
						source.MemberName,
						(v, ov) => source.SetValue(v),
						source.GetValue() ?? PrimitiveTypes[i].@default);
				}
			}

			return null;
		}

		private DynamicMock GetOrCreateMock(ValueStorage settability)
		{
			if (settability.GetValue() is DynamicMock subject)
			{
			}
			else
			{
				var type = settability.Type;
				var properties = type.GetTypeInfo().DeclaredProperties.ToArray();
				var fields = type.GetTypeInfo().DeclaredFields.ToArray();
				subject = new DynamicMock(properties, fields);
				settability.SetValue(subject);
			}

			return subject;
		}
	}
}
