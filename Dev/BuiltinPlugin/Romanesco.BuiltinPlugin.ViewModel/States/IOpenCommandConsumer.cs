using Reactive.Bindings;

namespace Romanesco.BuiltinPlugin.ViewModel.States;

public interface IOpenCommandConsumer
{
	ReactiveCommand OnOpenCommand { get; }
}