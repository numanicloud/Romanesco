using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public class IntIdChoiceListState : SimpleStateBase
	{
		record Element(IntIdChoiceState State, IDisposable Subscription)
		{
			public static Element Create(IntIdChoiceState state, IObserver<Unit> onContentsChanged)
			{
				var subscription = state.OnEdited
					.Subscribe(_ => onContentsChanged.OnNext(Unit.Default));
				return new Element(state, subscription);
			}
		}

		private readonly string _masterName;
		private readonly MasterListContext _masterList;
		private readonly CommandHistory _history;
		private readonly Subject<Unit> _onContentsChanged = new();
		private readonly ObservableCollection<Element> _elements = new();
		private readonly List<int> _listInstance;
		private readonly ValueStorageFactory _valueStorageFactory = new();

		public ReactiveProperty<MasterList?> Master { get; } = new(mode:ReactivePropertyMode.RaiseLatestValueOnSubscribe);
		public override IReadOnlyReactiveProperty<string> FormattedString { get; }
		public ReadOnlyReactiveCollection<IntIdChoiceState> Elements { get; } 
		
		public IntIdChoiceListState(ValueStorage storage, string masterName, MasterListContext masterList, CommandHistory history)
			: base(storage)
		{
			_masterName = masterName;
			_masterList = masterList;
			_history = history;

			if (storage.GetValue() is List<int> list)
			{
				_listInstance = list;
			}
			else
			{
				_listInstance = new List<int>();
				storage.SetValue(_listInstance);
			}

			// マスターのデータは遅れて初期化される可能性があるので、初期化まで待ってから選択肢を更新する
			masterList.OnKeyAdded.Where(key => masterName == key)
				.Subscribe(key => UpdateChoices(masterName, masterList))
				.AddTo(Disposables);

			UpdateChoices(masterName, masterList);

			Elements = _elements.ToReadOnlyReactiveCollection(x => x.State);

			OnEdited = _onContentsChanged;

			foreach (var item in _listInstance)
			{
				var index = _elements.Count;
				var settability = _valueStorageFactory.FromListElement(typeof(int), _listInstance, index.ToString(), item);
				var state = new IntIdChoiceState(settability, masterName, masterList);

				_elements.Add(Element.Create(state, _onContentsChanged));
			}

			FormattedString = _onContentsChanged.Select(_ => $"Count = {_elements.Count}")
				.ToReadOnlyReactiveProperty($"Count = {_elements.Count}");
		}

		public void AddNewElement()
		{
			Insert(0, _elements.Count);
		}

		public void Insert(int value, int index)
		{
			var storage = _valueStorageFactory.FromListElement(typeof(int), _listInstance,
				index.ToString(), value);
			var state = new IntIdChoiceState(storage, _masterName, _masterList);
			state.Master.Value = Master.Value;

			_elements.Insert(index, Element.Create(state, _onContentsChanged));
			_listInstance.Insert(index, value);
			_onContentsChanged.OnNext(Unit.Default);
		}

		public void RemoveAt(int index)
		{
			_elements[index].Subscription.Dispose();

			_elements.RemoveAt(index);
			_listInstance.RemoveAt(index);
			_onContentsChanged.OnNext(Unit.Default);

			for (int i = index; i < _elements.Count; i++)
			{
				_elements[i].State.Title.Value = i.ToString();
			}
		}

		private void UpdateChoices(string masterName, MasterListContext context)
		{
			if (context.Masters.TryGetValue(masterName, out var list) && list.IdType == typeof(int))
			{
				Master.Value = list;
			}
			else
			{
				Master.Value = null;
			}
		}

		public override void Dispose()
		{
			Master.Dispose();
			Elements.Dispose();
			base.Dispose();
		}
	}
}
