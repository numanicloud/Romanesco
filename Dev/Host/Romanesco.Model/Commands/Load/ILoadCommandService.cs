using System;
using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.EditorComponents.States;

namespace Romanesco.Model.Commands
{
	internal interface ILoadCommandService : ICommandModel
	{
		IObservable<IProjectContext> OnExecuted { get; }

		Task<IProjectContext?> ExecuteAsync();

		void UpdateState(IEditorState state);
	}
}
