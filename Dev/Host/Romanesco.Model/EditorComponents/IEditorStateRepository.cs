using Reactive.Bindings;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.EditorComponents
{
	internal interface IEditorStateRepository
	{
		IReadOnlyReactiveProperty<IEditorState> EditorState { get; }
	}
}
