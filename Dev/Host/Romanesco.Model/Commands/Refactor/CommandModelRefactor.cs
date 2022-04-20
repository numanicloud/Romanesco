using System.Threading.Tasks;
using Reactive.Bindings;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands.Refactor
{
	public abstract class CommandModelRefactor
	{
		public ReactiveProperty<bool> CanExecute { get; } = new();
		internal abstract Task Execute(IEditorState state);
		internal abstract void AfterExecute(IEditorState state);
	}
}
