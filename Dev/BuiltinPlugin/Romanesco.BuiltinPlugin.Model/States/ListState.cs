using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public sealed class ListState : SimpleStateBase
	{
		public class Element : IDisposable
		{
			private Element(IFieldState state, IDisposable subscription)
			{
				State = state;
				Subscription = subscription;
			}

			public IFieldState State { get; }
			public IDisposable Subscription { get; }

			public void Dispose()
			{
				State.Dispose();
				Subscription.Dispose();
			}

			public static Element Create(IFieldState state, IObserver<Unit> onContentsChanged)
			{
				state.Storage.OnValueChanged
					.Subscribe(_ => Debug.WriteLine("Element-Write", "Romanesco"));
				var disposable = state.OnEdited
					.Subscribe(value => onContentsChanged.OnNext(Unit.Default));
				return new Element(state, disposable);
			}
		}

		private readonly Subject<Unit> onContentsChanged = new Subject<Unit>();
		private readonly ObservableCollection<Element> elementsMutable;
		private readonly StateInterpretFunc interpret;
		private readonly CommandHistory history;
		private readonly IList listInstance;
		private readonly ValueStorageFactory valueStorageFactory = new ValueStorageFactory();

		public override IReadOnlyReactiveProperty<string> FormattedString { get; }
		public Type ElementType { get; set; }
		public ObservableCollection<IFieldState> Elements { get; }

		public ListState(ValueStorage storage, IList listInstance, StateInterpretFunc interpret, CommandHistory history) : base(storage)
		{
			this.listInstance = listInstance;
			this.interpret = interpret;
			this.history = history;
			elementsMutable = new ObservableCollection<Element>();
			Elements = elementsMutable.ToObservableCollection(t => t.State).result;
			ElementType = Storage.Type.GetGenericArguments()[0];

			FormattedString = onContentsChanged.Select(_ => $"Count = {elementsMutable.Count}")
				.ToReadOnlyReactiveProperty("Count = 0");

			LoadInitialValue(listInstance);

			OnEdited = onContentsChanged;
		}

		private void LoadInitialValue(IList loadedListInstance)
		{
			for (int i = 0; i < loadedListInstance.Count; i++)
			{
				var state = MakeState(i.ToString(), loadedListInstance[i]);
				if (state is null)
				{
					continue;
				}
				elementsMutable.Add(Element.Create(state, onContentsChanged));
			}
			onContentsChanged.OnNext(Unit.Default);
		}

		public IFieldState? AddNewElement()
		{
			object? defaultValue = ElementType.IsValueType ? Activator.CreateInstance(ElementType) : null;
			var i = elementsMutable.Count;
			var state = MakeState(i.ToString(), defaultValue);

			if (state is null)
			{
				return null;
			}

			Insert(state, i);
			return state;
		}

		private IFieldState? MakeState(string title, object? initialValue)
		{
			var storage = valueStorageFactory.FromListElement(ElementType, listInstance, title, initialValue);
			var state = interpret(storage);
			if (state == null)
			{
				OnErrorSubject.OnNext(new InvalidOperationException($"{ElementType}型のデータモデルを生成できませんでした。"));
				return null;
			}

			return state;
		}

		public void Insert(IFieldState state, int index)
		{
			elementsMutable.Insert(index, Element.Create(state, onContentsChanged));
			listInstance.Insert(index, state.Storage.GetValue());
			onContentsChanged.OnNext(Unit.Default);

			if (!history.IsOperating)
			{
				var memento = new AddElementToListCommandMemento(this, state, index);
				history.PushMemento(memento);
			}
		}

		public void RemoveAt(int index)
		{
			var state = elementsMutable[index].State;

			elementsMutable[index].Subscription.Dispose();	// StateはUndo時に使うかもしれない

			elementsMutable.RemoveAt(index);
			listInstance.RemoveAt(index);
			onContentsChanged.OnNext(Unit.Default);

			for (int i = index; i < elementsMutable.Count; i++)
			{
				elementsMutable[i].State.Title.Value = i.ToString();
			}

			if (!history.IsOperating)
			{
				var memento = new RemoveElementToListCommandMemento(this, state, index);
				history.PushMemento(memento);
			}
		}

		public override void Dispose()
		{
			onContentsChanged.Dispose();
			elementsMutable.ForEach(x => x.Dispose());
			base.Dispose();
		}
	}

}
