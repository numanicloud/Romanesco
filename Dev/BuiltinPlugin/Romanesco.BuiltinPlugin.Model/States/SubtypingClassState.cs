using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.States;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using System.Reactive.Linq;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public class SubtypingClassState : IFieldState
	{
		private ReactiveProperty<IFieldState> CurrentState { get; set; } = new ReactiveProperty<IFieldState>();

		public ObservableCollection<Type> Choices { get; } = new ObservableCollection<Type>();

		public ReactiveProperty<Type?> SelectedType { get; } = new ReactiveProperty<Type?>();

		public IReactiveProperty<IFieldState> CurrentStateReadOnly => CurrentState;


		public ReactiveProperty<string> Title => CurrentState.Value.Title;

		public IReadOnlyReactiveProperty<string> FormattedString => CurrentState.Value.Title;

		public Type Type => CurrentState.Value.Type;

		public ValueStorage Storage => CurrentState.Value.Storage;

		public IObservable<Exception> OnError => CurrentState.Value.OnError;

		public IObservable<Unit> OnEdited => CurrentState.Value.OnEdited;

		public SubtypingClassState(ValueStorage storage, SubtypingList subtypingList, StateInterpretFunc interpreter)
		{
			// 型の選択肢を設定
			foreach (var item in subtypingList.DerivedTypes)
			{
				Choices.Add(item);
			}
			subtypingList.OnNewEntry.Subscribe(x => Choices.Add(x));

			// 型の初期値をセット
			TrySetInstance(storage.GetValue()?.GetType(), storage, interpreter, out var state);
			CurrentState.Value = state;

			// 型が変更されたら更新
			SelectedType.Subscribe(x =>
			{
				if (!TrySetInstance(x, storage, interpreter, out var state))
				{
					// ClassStateとして生成できないなら選択肢にはありえないので
					SelectedType.Value = null;
					if (x is Type)
					{
						Choices.Remove(x);
					}
				}
				CurrentState.Value = state;
			});
		}

		private bool TrySetInstance(Type? derivedType, ValueStorage me, StateInterpretFunc interpreter,
			out IFieldState result)
		{
			if (derivedType is Type
				&& Activator.CreateInstance(derivedType) is object instance)
			{
				// 元の ValueStorage とは型の抽象度が異なる新しい ValueStorage
				var concreteStorage = new ValueStorage(derivedType,
					me.MemberName,
					(value, old) => me.SetValue(value),
					instance);

				if (interpreter(concreteStorage) is ClassState state)
				{
					result = state;
					return true;
				}
			}
			result = new NoneState();
			return false;
		}
	}
}
