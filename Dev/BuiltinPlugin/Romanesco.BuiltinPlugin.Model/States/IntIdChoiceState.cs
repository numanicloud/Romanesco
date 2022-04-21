using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public class IntIdChoiceState : SimpleStateBase
	{
		public override IReadOnlyReactiveProperty<string> FormattedString { get; }
		public ReactiveProperty<MasterList?> Master { get; } = new();
		public ReactiveProperty<IFieldState?> SelectedItem { get; } = new (mode:ReactivePropertyMode.None);

		public IntIdChoiceState(ValueStorage storage, string masterName, MasterListContext context) : base(storage)
		{
			context.OnKeyAdded.Where(key => masterName == key)
				.Subscribe(key => UpdateChoices(key, context))
				.AddTo(Disposables);

			UpdateChoices(masterName, context);

			FormattedString = SelectedItem.Select(SanitizeNewFormattedString)
				.ToReactiveProperty(storage.GetValue()?.ToString() ?? "");

			// �l�̕ω��𐶃f�[�^�֔��f
			SelectedItem.Subscribe(x =>
			{
				var id = Master.Value == null ? -1
					: x?.Storage.GetValue() is { } value ? Master.Value.GetId(value)
					: -1;
				Storage.SetValue(id);
			}).AddTo(Disposables);
		}

		// �ҏW���͌Ă΂�Ȃ����A���[�h���͂���State����Ƀ}�X�^�[���ǂݍ��܂��ꍇ������̂Œx���ł���悤��
		private void UpdateChoices(string masterName, MasterListContext context)
		{
			if (context.Masters.TryGetValue(masterName, out var list) && list.IdType == typeof(int))
			{
				Master.Value = list;
				try
				{
					SelectedItem.Value = list.GetById(Storage.GetValue() ?? -1);
				}
				catch (KeyNotFoundException)
				{
					SelectedItem.Value = null;
				}
			}
			else
			{
				Master.Value = null;
			}
		}

		public override void Dispose()
		{
			Master.Dispose();
			SelectedItem.Dispose();
			base.Dispose();
		}
	}
}