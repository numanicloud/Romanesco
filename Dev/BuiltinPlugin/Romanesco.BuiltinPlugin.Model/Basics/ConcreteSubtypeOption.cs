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
			if (!(context.AsmRepo.CreateInstance(derivedType) is { } instance))
			{
				throw MakeException($"型 {derivedType.FullName} のインスタンスを作成できません。");
			}

			valueStorage.SetValue(instance);

			// 最基底の型を生成しようとしてしまっている。再派生の型を生成すべき
			// 新しいValueStorageを作らないようにしたせいかも
			var returnValue = _factory.InterpretAsState(
					valueStorage.Clone(derivedType),
					context.Interpreter.InterpretAsState) is {} result
				? result
				: throw new InvalidOperationException();

			return returnValue;
		}
		
		private Exception MakeException(string message) => new InvalidOperationException(message
			+ $"Subtyping: MemberName={subtypingStorage.MemberName}, Type={subtypingStorage.Type.FullName}");
	}
}
