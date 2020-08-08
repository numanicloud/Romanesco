using Reactive.Bindings;

namespace Romanesco.Model.Interfaces
{
	public interface ICommandAvailabilityPublisher
	{
		IReadOnlyReactiveProperty<bool> CanSave { get; }
		IReadOnlyReactiveProperty<bool> CanSaveAs { get; }
		IReadOnlyReactiveProperty<bool> CanExport { get; }
		IReadOnlyReactiveProperty<bool> CanCreate { get; }
		IReadOnlyReactiveProperty<bool> CanOpen { get; }
		IReadOnlyReactiveProperty<bool> CanUndo { get; }
		IReadOnlyReactiveProperty<bool> CanRedo { get; }
	}
}
