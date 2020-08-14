using Reactive.Bindings;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.EditorComponents
{
	interface IEditorStateRepository
	{
		IReadOnlyReactiveProperty<IEditorState> EditorState { get; }
	}
}
