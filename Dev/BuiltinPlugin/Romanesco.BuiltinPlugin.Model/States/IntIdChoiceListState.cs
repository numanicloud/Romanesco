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
		private readonly CommandHistory _history;
		private readonly Subject<Unit> _onContentsChanged = new();
		private readonly ObservableCollection<ListState.Element> _elements = new();
		private readonly List<int> _listInstance;
		private readonly ValueStorageFactory _valueStorageFactory = new();

		public ReactiveProperty<MasterList?> Master { get; } = new();
		public ReadOnlyReactiveCollection<string?> SelectedItemStrings { get; }
		public override IReadOnlyReactiveProperty<string> FormattedString { get; }

		public IntIdChoiceListState(ValueStorage storage, string masterName, MasterListContext masterList, CommandHistory history)
			: base(storage)
		{
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

			SelectedItemStrings = _onContentsChanged
				.SelectMany(_ => _listInstance.Select(x => Master.Value?.State.Elements[x].ToString()))
				.ToReadOnlyReactiveCollection();

			FormattedString = _onContentsChanged.Select(_ => $"Count = {_elements.Count}")
				.ToReadOnlyReactiveProperty("Count = 0");
		}

		public IReadOnlyReactiveProperty<IFieldState?> AddNewElement()
		{
			var state = Insert(0, _elements.Count);
			return state.OnEdited
				.Select(_ => Master.Value?.State.Elements[state.PrimitiveContent.Value])
				.ToReadOnlyReactiveProperty(null);
		}

		public IntState Insert(int value, int index)
		{
			var storage = _valueStorageFactory.FromListElement(typeof(int), _listInstance,
				index.ToString(), value);
			var state = new IntState(storage, _history);

			_elements.Insert(index, ListState.Element.Create(state, _onContentsChanged));
			_listInstance.Insert(index, value);
			_onContentsChanged.OnNext(Unit.Default);

			return state;
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
