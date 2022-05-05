using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.Basics;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public class SubtypingClassState : DecorationStateBase
	{
		public ObservableCollection<ISubtypeOption> Choices { get; } = new ObservableCollection<ISubtypeOption>();
		public ReactiveProperty<ISubtypeOption> SelectedType { get; } = new ReactiveProperty<ISubtypeOption>();
		public IReactiveProperty<IFieldState> CurrentStateReadOnly => BaseField;
		public override ReactiveProperty<string> Title { get; }
		public override IReadOnlyReactiveProperty<string> FormattedString { get; }
		public override IObservable<Unit> OnEdited { get; }

		public SubtypingClassState(ValueStorage storage,
			SubtypingStateContext context,
			ClassStateFactory classStateFactory)
			: base(new ClassState(storage, Array.Empty<IFieldState>()))
		{
			void AddChoice(Type type)
			{
				Choices.Add(new ConcreteSubtypeOption(type, storage, context, classStateFactory));
			}

			Title = new ReactiveProperty<string>(storage.MemberName);

			// 型の選択肢を設定
			Choices.Add(new NullSubtypeOption(storage));
			context.List.DerivedTypes.ForEach(AddChoice);
			context.List.OnNewEntry.Subscribe(AddChoice).AddTo(Disposables);

			// 型の初期値をセット
			SelectedType.Value = storage.GetValue()?.GetType() is { } initialType
				? Choices.First(x => x.IsTypeOf(initialType))
				: Choices[0];

			// 型が変更されたら更新
			SelectedType.Subscribe(x =>
			{
				Console.WriteLine(storage == BaseField.Value.Storage);
				Debug.WriteLine("SelectedType Changed", "Romanesco");

				// ここでNullSubtypeOptionの特質によりNoneStateが生成されることによって、
				// Storageの値がおかしくなる
				BaseField.Value = x.MakeState(storage);

				// 以下の行がツリー構造からデータを外しているっぽい
				// この行を境に、storage == BaseField.Value.Storage が false になる
				
			}).AddTo(Disposables);

			FormattedString = BaseField.SelectMany(x => x.FormattedString)
				.ToReadOnlyReactiveProperty(BaseField.Value.FormattedString.Value);

			// 型の変更と、中身の変更は上に通知される
			var typeEdited = SelectedType.Select(x => Unit.Default);
			var concreteEdited = BaseField.SelectMany(x => x.OnEdited);
			OnEdited = typeEdited.Merge(concreteEdited);

			Debug.WriteLine("SubtypingClassState", "Romanesco");

			storage.OnValueChanged.Subscribe(_ =>
				Debug.WriteLine("SubtypingClassState.OnValueChanged", "Romanesco"));

			concreteEdited.Subscribe(_ =>
				Debug.WriteLine("SubtypingClassState.concreteEdited", "Romanesco"));
		}

		public override void Dispose()
		{
			SelectedType.Dispose();
			Title.Dispose();
			FormattedString.Dispose();
			base.Dispose();
		}
	}
}
