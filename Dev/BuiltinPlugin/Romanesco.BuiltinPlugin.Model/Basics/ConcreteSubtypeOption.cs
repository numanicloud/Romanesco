using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Romanesco.BuiltinPlugin.Model.Basics
{
	public class ConcreteSubtypeOption : ISubtypeOption
	{
		private readonly Type derivedType;
		private readonly ValueStorage subtypingStorage;
		private readonly StateInterpretFunc interpreter;

		public string OptionName { get; }

		public ConcreteSubtypeOption(Type derivedType, ValueStorage subtypingStorage, StateInterpretFunc interpreter)
		{
			this.derivedType = derivedType;
			this.subtypingStorage = subtypingStorage;
			this.interpreter = interpreter;
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
			if (Activator.CreateInstance(derivedType) is object instance)
			{
				var me = subtypingStorage;

				// 元の ValueStorage とは型の抽象度が異なる新しい ValueStorage
				var concreteStorage = new ValueStorage(derivedType,
					me.MemberName,
					(value, old) => me.SetValue(value),
					instance);

				if (me.GetValue() is { } value
					&& value.GetType() is Type loadedType
					&& loadedType == derivedType)
				{
					concreteStorage.SetValue(value);
				}

				if (interpreter(concreteStorage) is ClassState state)
				{
					me.SetValue(instance);
					return state;
				}
				else
				{
					throw MakeException($"型 {derivedType.FullName} はクラス型ではありません。");
				}
			}
			else
			{
				throw MakeException($"型 {derivedType.FullName} のインスタンスを作成できません。");
			}
		}
		
		private Exception MakeException(string message) => new InvalidOperationException(message
			+ $"Subtyping: MemberName={subtypingStorage.MemberName}, Type={subtypingStorage.Type.FullName}");
	}
}
