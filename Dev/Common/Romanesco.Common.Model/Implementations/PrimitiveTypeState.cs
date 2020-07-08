using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Exceptions;
using System;
using System.Reactive.Linq;

namespace Romanesco.Common.Model.Implementations
{
	public class PrimitiveTypeState<T> : SimpleStateBase
	{
		public override IReadOnlyReactiveProperty<string> FormattedString { get; }
		public ReactiveProperty<T> PrimitiveContent { get; } = new ReactiveProperty<T>();

		public PrimitiveTypeState(ValueStorage storage, CommandHistory history) : base(storage)
		{
			// 初期値を読み込み、変更を反映する処理を登録
			// TODO: !演算子を使わないためにも、class版とstruct版のPrimitiveTypeStateが必要
			PrimitiveContent.Value = (T) storage.GetValue()!;
			PrimitiveContent.Subscribe(value => Storage.SetValue(value))
				.AddTo(Disposables);

			FormattedString = GetFormattedStringObservable()
				.ToReadOnlyReactiveProperty(SelectFormattedString(PrimitiveContent.Value));

			// Undo/Redo登録
			storage.OnValueChangedWithOldValue
				.Where(_ => !history.IsOperating)
				.Select(t => new ContentEditCommandMemento(x => PrimitiveContent.Value = (T) x!, t.old, t.value))
				.Subscribe(history.PushMemento)
				.AddTo(Disposables);
		}

		private IObservable<string> GetFormattedStringObservable()
		{
			return PrimitiveContent.Select(x =>
			{
				try
				{
					return SelectFormattedString(x);
				}
				catch (Exception ex)
				{
					OnErrorSubject.OnNext(ContentAccessException.GetFormattedStringError(ex));
					return "";
				}
			});
		}

		protected virtual string SelectFormattedString(T value)
		{
			return value?.ToString() ?? "";
		}

		public override void Dispose()
		{
			PrimitiveContent.Dispose();
			base.Dispose();
		}
	}
}
