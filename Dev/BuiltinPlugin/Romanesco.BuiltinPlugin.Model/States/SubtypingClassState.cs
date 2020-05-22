using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.States;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

namespace Romanesco.BuiltinPlugin.Model.States
{
	class SubtypingClassState : IFieldState
	{
		private IFieldState CurrentState { get; set; }


		public ObservableCollection<Type> Choices { get; } = new ObservableCollection<Type>();

		public ReactiveProperty<Type?> SelectedType { get; } = new ReactiveProperty<Type?>();

		public ReactiveProperty<string> Title => CurrentState.Title;

		public IReadOnlyReactiveProperty<string> FormattedString => CurrentState.Title;

		public Type Type => CurrentState.Type;

		public ValueStorage Storage => CurrentState.Storage;

		public IObservable<Exception> OnError => CurrentState.OnError;

		public IObservable<Unit> OnEdited => CurrentState.OnEdited;

		public SubtypingClassState(ValueStorage storage, SubtypingList subtypingList, StateInterpretFunc interpreter)
		{
			CurrentState = new NoneState();

			foreach (var item in subtypingList.DerivedTypes)
			{
				Choices.Add(item);
			}

			subtypingList.OnNewEntry.Subscribe(x => Choices.Add(x));

			SelectedType.Subscribe(x =>
			{
				if (x is Type derivedType
					&& Activator.CreateInstance(derivedType) is object instance)
				{
					storage.SetValue(instance);
					if (interpreter(storage) is ClassState state)
					{
						CurrentState = state;
					}
					else
					{
						// ClassStateとして生成できないなら選択肢にはありえないので
						Choices.Remove(x);
						SelectedType.Value = null;
						storage.SetValue(null);
					}
				}
			});
		}
	}
}
