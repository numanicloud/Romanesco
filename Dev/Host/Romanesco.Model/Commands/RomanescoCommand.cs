using System.Threading.Tasks;
using Reactive.Bindings;
using Romanesco.Model.States;

namespace Romanesco.Model.Commands
{
	public abstract class RomanescoCommand
	{
		public ReactiveProperty<bool> CanExecute { get; } = new();
		internal abstract Task Execute(IEditorState state);
		internal abstract void AfterExecute(IEditorState state);
	}
}
