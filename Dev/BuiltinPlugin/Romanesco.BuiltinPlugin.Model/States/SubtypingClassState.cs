using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.Basics;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.States;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public class SubtypingClassState : IFieldState
	{
		private ReactiveProperty<IFieldState> CurrentState { get; set; } = new ReactiveProperty<IFieldState>();

		public ObservableCollection<ISubtypeOption> Choices { get; } = new ObservableCollection<ISubtypeOption>();

		public ReactiveProperty<ISubtypeOption> SelectedType { get; } = new ReactiveProperty<ISubtypeOption>();

		public IReactiveProperty<IFieldState> CurrentStateReadOnly => CurrentState;


		public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();

		public IReadOnlyReactiveProperty<string> FormattedString { get; }

		public Type Type => CurrentState.Value.Type;

		public ValueStorage Storage => CurrentState.Value.Storage;

		public IObservable<Exception> OnError => CurrentState.Value.OnError;

		public IObservable<Unit> OnEdited { get; }

		public SubtypingClassState(ValueStorage storage, SubtypingList subtypingList, StateInterpretFunc interpreter)
		{
			// 型の選択肢を設定
			Choices.Add(new NullSubtypeOption());
			foreach (var item in subtypingList.DerivedTypes)
			{
				Choices.Add(new ConcreteSubtypeOption(item, storage, interpreter));
			}
			subtypingList.OnNewEntry.Subscribe(x => Choices.Add(new ConcreteSubtypeOption(x, storage, interpreter)));

			// 型の初期値をセット
			var initialType = storage.GetValue()?.GetType();
			if (initialType is { })
			{
				SelectedType.Value = Choices.First(x => x.IsTypeOf(initialType));
			}
			else
			{
				// 上記のように、 0 番目は NullSubtypeOption
				SelectedType.Value = Choices[0];
			}

			// 型が変更されたら更新
			SelectedType.Subscribe(x =>
			{
				var state = x.MakeState();
				CurrentState.Value = state;

				if (!(state is ClassState || x is NullSubtypeOption))
				{
					// ClassStateとして生成できないなら選択肢にはありえないので
					SelectedType.Value = Choices[0];
					Choices.Remove(x);
				}
			});

			Title.Value = storage.MemberName;

			FormattedString = CurrentState
				.SelectMany(x => x.FormattedString)
				.ToReadOnlyReactiveProperty("");

			// 型の変更と、中身の変更は上に通知される
			var typeEdited = SelectedType.Select(x => Unit.Default);
			var concreteEdited = CurrentState.SelectMany(x => x.OnEdited);
			OnEdited = typeEdited.Merge(concreteEdited);
		}
	}
}
