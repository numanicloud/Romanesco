using Reactive.Bindings;

namespace Romanesco.Common.Model.Basics;

public class ValueClipBoard
{
	private ReactiveProperty<ValueStorage?> _copiedValue = new();

	public IReadOnlyReactiveProperty<ValueStorage?> CopiedValue => _copiedValue;

	public void Copy(ValueStorage value)
	{
		_copiedValue.Value = value.Clone(value.Type);
	}
}