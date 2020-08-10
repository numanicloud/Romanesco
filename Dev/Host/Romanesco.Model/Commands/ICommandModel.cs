using System.Threading.Tasks;
using Reactive.Bindings;

namespace Romanesco.Model.Commands
{
	interface ICommandModel
	{
		IReadOnlyReactiveProperty<bool> CanExecute { get; }
	}
}
