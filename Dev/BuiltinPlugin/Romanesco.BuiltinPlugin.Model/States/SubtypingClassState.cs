using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.Basics;
using Romanesco.BuiltinPlugin.Model.Factories;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Implementations;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public class SubtypingClassState : DecorationStateBase
	{
		private readonly ValueClipBoard _valueClipBoard;
		private BooleanDisposable? _valueChanging;

		public ObservableCollection<ISubtypeOption> Choices { get; } = new ObservableCollection<ISubtypeOption>();
		public ReactiveProperty<ISubtypeOption> SelectedType { get; } = new ReactiveProperty<ISubtypeOption>();
		public IReactiveProperty<IFieldState> CurrentStateReadOnly => BaseField;
		public override ReactiveProperty<string> Title { get; }
		public override IReadOnlyReactiveProperty<string> FormattedString { get; }
		public override IObservable<Unit> OnEdited { get; }
		public IReadOnlyReactiveProperty<bool> CanPaste { get; }

		public SubtypingClassState(ValueStorage storage,
			SubtypingStateContext context,
			ClassStateFactory classStateFactory,
			ValueClipBoard valueClipBoard,
			IStorageCloneService storageCloneService)
			: base(new ClassState(storage, Array.Empty<IFieldState>()))
		{
			_valueClipBoard = valueClipBoard;
			CanPaste = _valueClipBoard.CopiedValue
				.Where(x => x is not null)
				.Select(x => context.List.DerivedTypes.Contains(x!.Type))
				.ToReadOnlyReactiveProperty();

			void AddChoice(Type type)
			{
				Choices.Add(new ConcreteSubtypeOption(type, context, classStateFactory, storageCloneService));
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
			SelectedType.Where(_ => _valueChanging?.IsDisposed != false)
				.Subscribe(x =>
			{
				BaseField.Value = x.MakeState(storage);
			}).AddTo(Disposables);

			// 値が変更されたら更新
			FormattedString = BaseField.SelectMany(x => x.FormattedString)
				.ToReadOnlyReactiveProperty(BaseField.Value.FormattedString.Value);

			// 型の変更と、中身の変更は上に通知される
			var typeEdited = SelectedType.Select(x => Unit.Default);
			var concreteEdited = BaseField.SelectMany(x => x.OnEdited);
			OnEdited = typeEdited.Merge(concreteEdited);
		}

		public void Copy()
		{
			_valueClipBoard.Copy(Storage);
		}

		public void Paste()
		{
			_valueChanging = new BooleanDisposable();

			var value = _valueClipBoard.CopiedValue.Value;
			if (value is null) return;

			var option = Choices.FirstOrDefault(x => x.IsTypeOf(value.Type));
			if (option is null) return;
			
			BaseField.Value.Dispose();
			BaseField.Value = option.MakeFromStorage(Storage, value);
			SelectedType.Value = option;

			_valueChanging.Dispose();
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
