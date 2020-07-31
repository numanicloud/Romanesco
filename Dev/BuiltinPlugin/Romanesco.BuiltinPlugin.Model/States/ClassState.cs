using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System.Linq;
using System.Reactive.Linq;
using Romanesco.Common.Model.Implementations;

namespace Romanesco.BuiltinPlugin.Model.States
{
	public class ClassState : SimpleStateBase
	{
		public IFieldState[] Fields { get; }
		public override IReadOnlyReactiveProperty<string> FormattedString { get; }

		public ClassState(ValueStorage storage, IFieldState[] fields) : base(storage)
		{
			Fields = fields;
			OnEdited = fields.Select(x => x.OnEdited).Merge();
			FormattedString = OnEdited
				.Select(_ => SanitizeNewFormattedString(Storage.GetValue()))
				.ToReadOnlyReactiveProperty(storage.GetValue()?.ToString() ?? "");
		}

		public override void Dispose()
		{
			foreach (var state in Fields)
			{
				state.Dispose();
			}
			base.Dispose();
		}
	}
}
