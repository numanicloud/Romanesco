using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public class ListState : IFieldState
	{
		private readonly Subject<Unit> onContentsChanged = new Subject<Unit>();
		private readonly ObservableCollection<(IFieldState state, IDisposable disposable)> elementsMutable;
		private readonly StateInterpretFunc interpret;
		private readonly CommandHistory history;
		private readonly IList listInstance;

		public ReactiveProperty<string> Title { get; }
		public IReadOnlyReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
		public Type Type => Storage.Type;
		public Type ElementType { get; }
		public ValueStorage Storage { get; }
		public ObservableCollection<IFieldState> Elements { get; }
		public IObservable<Exception> OnError => Observable.Never<Exception>();
		public IObservable<Unit> OnEdited => onContentsChanged;

		public ListState(ValueStorage storage, StateInterpretFunc interpret, CommandHistory history)
		{
			Title = new ReactiveProperty<string>(storage.MemberName);
			elementsMutable = new ObservableCollection<(IFieldState state, IDisposable disposable)>();
			Elements = elementsMutable.ToObservableCollection(t => t.state).result;

			Storage = storage;
			this.interpret = interpret;
			this.history = history;

			ElementType = Storage.Type.GetGenericArguments()[0];

			FormattedString = onContentsChanged.Select(_ => $"Count = {elementsMutable.Count}")
				.ToReadOnlyReactiveProperty("Count = 0");

			// 初期値を読み込み。初期値が無かったら生成
			if (Storage.GetValue() is IList li)
			{
				listInstance = li;
				LoadInitialValue(listInstance, interpret);
			}
			else if (Activator.CreateInstance(typeof(List<>).MakeGenericType(ElementType)) is IList newList)
			{
				listInstance = newList;
				Storage.SetValue(listInstance);
			}
			else
			{
				throw new InvalidOperationException($"{storage.Type.FullName}のインスタンスを生成できませんでした。");
			}
		}

		private void LoadInitialValue(IList loadedListInstance, StateInterpretFunc interpret)
		{
			for (int i = 0; i < loadedListInstance.Count; i++)
			{
				MakeState(loadedListInstance, interpret, i);
			}
			onContentsChanged.OnNext(Unit.Default);
		}

		public IFieldState AddNewElement()
		{
			var i = elementsMutable.Count;
			listInstance.Add(GetDefaultValue());

			var state = MakeState(listInstance, interpret, i);

			onContentsChanged.OnNext(Unit.Default);

			if (!history.IsOperating)
			{
				var memento = new AddElementToListCommandMemento(this, state, i);
				history.PushMemento(memento);
			}

			return state;
		}

		private IFieldState MakeState(IList listInstance, StateInterpretFunc interpret, int stateIndex)
		{
			var storage = new ValueStorage(
				ElementType,
				$"{stateIndex}",
				(value, oldValue) =>
				{
					var index = listInstance.IndexOf(oldValue);
					listInstance[index] = value;
				},
				listInstance[stateIndex] ?? throw new IndexOutOfRangeException());

			var state = interpret(storage);
			if (state == null)
			{
				throw new InvalidOperationException($"{ElementType}型のデータモデルを生成できませんでした。");
			}

			var disposable = SubscribeElementState(state);
			elementsMutable.Add((state, disposable));

			return state;
		}

		private object? GetDefaultValue()
		{
			if (ElementType.IsValueType)
			{
				return Activator.CreateInstance(ElementType);
			}

			return null;
		}

		private IDisposable SubscribeElementState(IFieldState state)
		{
			return state.OnEdited
				.Where(value => value != null)
				.Subscribe(value => onContentsChanged.OnNext(Unit.Default));
		}

		public void Insert(IFieldState state, int index)
		{
			listInstance.Insert(index, state.Storage.GetValue());

			var disposable = SubscribeElementState(state);
			elementsMutable.Insert(index, (state, disposable));

			onContentsChanged.OnNext(Unit.Default);

			if (!history.IsOperating)
			{
				var memento = new AddElementToListCommandMemento(this, state, index);
				history.PushMemento(memento);
			}
		}

		public void RemoveAt(int index)
		{
			var state = elementsMutable[index].state;

			elementsMutable[index].disposable.Dispose();
			elementsMutable.RemoveAt(index);
			listInstance.RemoveAt(index);
			onContentsChanged.OnNext(Unit.Default);

			for (int i = index; i < elementsMutable.Count; i++)
			{
				elementsMutable[i].state.Title.Value = i.ToString();
			}

			if (!history.IsOperating)
			{
				var memento = new RemoveElementToListCommandMemento(this, state, index);
				history.PushMemento(memento);
			}
		}
	}
}
