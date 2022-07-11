using Romanesco.Annotations;
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
		private readonly Type _derivedType;
		private readonly SubtypingStateContext _context;
		private readonly ClassStateFactory _factory;
		private readonly IStorageCloneService _storageCloneService;
		private readonly object? _initialValue;

		public string OptionName { get; }

		public ConcreteSubtypeOption(Type derivedType,
			SubtypingStateContext context, ClassStateFactory factory,
			IStorageCloneService storageCloneService)
		{
			_derivedType = derivedType;
			_context = context;
			_factory = factory;
			_storageCloneService = storageCloneService;
			if (derivedType.GetCustomAttribute<EditorSubtypeNameAttribute>() is { } attr)
			{
				OptionName = attr.OptionName;
			}
			else
			{
				OptionName = derivedType.FullName ?? throw new InvalidOperationException("型名を取得できませんでした。");
			}

			_initialValue = context.AsmRepo.CreateInstance(derivedType);
		}

		public bool IsTypeOf(Type type) => type == _derivedType;

		public IFieldState MakeState(ValueStorage valueStorage)
		{
			// 型が同じ場合は初期値である場合があるので上書きしない
			if (valueStorage.GetValue()?.GetType() is not {} current || current != _derivedType)
			{
				// この型の初期値で上書きする
				valueStorage.SetValue(_initialValue);
			}

				// 最基底の型を生成しようとしてしまっている。再派生の型を生成すべき
			// 新しいValueStorageを作らないようにしたせいかも
			var returnValue = _factory.InterpretAsState(
				valueStorage.Clone(_derivedType),
				_context.Interpreter.InterpretAsState) ?? throw new InvalidOperationException();

			return returnValue;
		}

		public IFieldState MakeFromStorage(ValueStorage valueStorage, ValueStorage pasteFrom)
		{
			if (pasteFrom.Type != _derivedType)
			{
				throw new InvalidOperationException();
			}

			// 与えられた値で上書きする
			valueStorage.SetValue(_storageCloneService.Clone(pasteFrom).GetValue());

			ValueStorage clone = valueStorage.Clone(_derivedType);
			return _factory.InterpretAsState(
				clone,
				_context.Interpreter.InterpretAsState) ?? throw new InvalidOperationException();
		}
	}
}
