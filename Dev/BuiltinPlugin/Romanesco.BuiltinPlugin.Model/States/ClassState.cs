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
		public record Property(string Name, IFieldState State);

		public IFieldState[] Children { get; }
		public Property[] Properties { get; }
		public override IReadOnlyReactiveProperty<string> FormattedString { get; }

		public ClassState(ValueStorage storage, Property[] properties) : base(storage)
		{
			Properties = properties;
			Children = properties.Select(x => x.State).ToArray();
			OnEdited = properties.Select(x => x.State.OnEdited).Merge();
			FormattedString = OnEdited
				.Select(_ => SanitizeNewFormattedString(Storage.GetValue()))
				.ToReadOnlyReactiveProperty(storage.GetValue()?.ToString() ?? "");
		}

		public override void Dispose()
		{
			foreach (var state in Children)
			{
				state.Dispose();
			}
			base.Dispose();
		}
	}
}
