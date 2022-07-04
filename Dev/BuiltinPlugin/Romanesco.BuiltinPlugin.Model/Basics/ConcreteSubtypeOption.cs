using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Reflection;
using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.BuiltinPlugin.Model.Infrastructure;

namespace Romanesco.BuiltinPlugin.Model.Basics
{
	public class ConcreteSubtypeOption : ISubtypeOption
	{
		private readonly Type derivedType;
		private readonly ValueStorage subtypingStorage;
		private readonly SubtypingStateContext context;
		private readonly ClassStateFactory _factory;

		public string OptionName { get; }

		public ConcreteSubtypeOption(Type derivedType, ValueStorage subtypingStorage,
			SubtypingStateContext context, ClassStateFactory factory)
		{
			this.derivedType = derivedType;
			this.subtypingStorage = subtypingStorage;
			this.context = context;
			_factory = factory;
			if (derivedType.GetCustomAttribute<EditorSubtypeNameAttribute>() is { } attr)
			{
				OptionName = attr.OptionName;
			}
			else
			{
				OptionName = derivedType.FullName ?? throw new InvalidOperationException("型名を取得できませんでした。");
			}
		}

		public bool IsTypeOf(Type type) => type == derivedType;

		public IFieldState MakeState(ValueStorage valueStorage)
		{
			valueStorage.SetValue(subtypingStorage.GetValue());

				// 最基底の型を生成しようとしてしまっている。再派生の型を生成すべき
			// 新しいValueStorageを作らないようにしたせいかも
			var returnValue = _factory.InterpretAsState(
				valueStorage.Clone(derivedType),
				context.Interpreter.InterpretAsState) ?? throw new InvalidOperationException();

			return returnValue;
		}

		public IFieldState MakeFromStorage(ValueStorage valueStorage)
		{
			if (valueStorage.Type != derivedType)
			{
				throw new InvalidOperationException();
			}

			return _factory.InterpretAsState(
				valueStorage,
				context.Interpreter.InterpretAsState) ?? throw new InvalidOperationException();
		}

		private Exception MakeException(string message) => new InvalidOperationException(message
			+ $"Subtyping: MemberName={subtypingStorage.MemberName}, Type={subtypingStorage.Type.FullName}");
	}
}
