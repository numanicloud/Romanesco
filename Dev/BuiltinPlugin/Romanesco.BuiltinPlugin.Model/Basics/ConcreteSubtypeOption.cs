using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Reflection;
using Romanesco.BuiltinPlugin.Model.Infrastructure;

namespace Romanesco.BuiltinPlugin.Model.Basics
{
	public class ConcreteSubtypeOption : ISubtypeOption
	{
		private readonly Type derivedType;
		private readonly ValueStorage subtypingStorage;
		private readonly SubtypingStateContext context;

		public string OptionName { get; }

		public ConcreteSubtypeOption(Type derivedType, ValueStorage subtypingStorage,
			SubtypingStateContext context)
		{
			this.derivedType = derivedType;
			this.subtypingStorage = subtypingStorage;
			this.context = context;
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

		public IFieldState MakeState()
		{
			if (!(context.AsmRepo.CreateInstance(derivedType) is { } instance))
			{
				throw MakeException($"型 {derivedType.FullName} のインスタンスを作成できません。");
			}

			var me = subtypingStorage;

			// 元の ValueStorage とは型の抽象度が異なる新しい ValueStorage
			var concreteStorage = new ValueStorage(derivedType,
				me.MemberName,
				(v, old) => me.SetValue(v),
				instance);

			if (me.GetValue() is { } value
			    && value.GetType() is { } loadedType
			    && loadedType == derivedType)
			{
				concreteStorage.SetValue(value);
			}

			if (!(context.Interpreter.InterpretAsState(concreteStorage) is ClassState state))
			{
				throw MakeException($"型 {derivedType.FullName} はクラス型ではありません。");
			}

			me.SetValue(instance);
			return state;
		}
		
		private Exception MakeException(string message) => new InvalidOperationException(message
			+ $"Subtyping: MemberName={subtypingStorage.MemberName}, Type={subtypingStorage.Type.FullName}");
	}
}
