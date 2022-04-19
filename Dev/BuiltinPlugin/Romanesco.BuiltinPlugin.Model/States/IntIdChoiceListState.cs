using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NacHelpers.Extensions;
using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Implementations;
using Romanesco.Common.Model.Interfaces;

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

		public ReactiveProperty<MasterList?> Master { get; } = new();
		public override IReadOnlyReactiveProperty<string> FormattedString { get; }

		public ReadOnlyReactiveCollection<IntIdChoiceState> ChoiceStates { get; }

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
				throw new Exception();
			}

			masterList.OnKeyAdded.Where(key => masterName == key)
				.Subscribe(key => UpdateChoices(masterName, masterList))
				.AddTo(Disposables);

			UpdateChoices(masterName, masterList);

			ChoiceStates = _elements.ToReadOnlyReactiveCollection(x => x.State);

			FormattedString = _onContentsChanged.Select(_ => $"Count = {_elements.Count}")
				.ToReadOnlyReactiveProperty("Count = 0");
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
			base.Dispose();
		}
	}
}
