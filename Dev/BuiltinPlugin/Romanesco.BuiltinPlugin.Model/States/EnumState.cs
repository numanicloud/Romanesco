using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Implementations;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings.Extensions;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public class EnumState : SimpleStateBase
	{
		public override IReadOnlyReactiveProperty<string> FormattedString { get; }
		public ReactiveProperty<object> Content { get; }
		public object[] Choices { get; }

		public EnumState(ValueStorage storage, CommandHistory history) : base(storage)
		{
			// Enum変数は初期値がnullでないので !演算子で済ませる
			Content = new ReactiveProperty<object>(storage.GetValue()!);
			Content.Subscribe(x => Storage.SetValue(x))
				.AddTo(Disposables);
			FormattedString = storage.OnValueChanged
				.Select(SanitizeNewFormattedString)
				.ToReactiveProperty(Content.Value.ToString() ?? "");

			var values = new List<object>();
			foreach (var value in Enum.GetValues(Type))
			{
				if (value != null)
				{
					values.Add(value);
				}
			}
			Choices = values.ToArray();

			if (Choices.Length == 0)
			{
				OnErrorSubject.OnNext(new Exception($"列挙体 {storage.Type.Name} に属する列挙子が 0 個でした。"));
				return;
			}

			// Undo/Redo登録
			storage.OnValueChangedWithOldValue
				.Where(_ => !history.IsOperating)
				.Select(t => new ContentEditCommandMemento(x => Storage.SetValue(x), t.old, t.value))
				.Subscribe(history.PushMemento)
				.AddTo(Disposables);
		}

		public override void Dispose()
		{
			base.Dispose();
			Content.Dispose();
		}
	}
}
