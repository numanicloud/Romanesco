using System.Threading.Tasks;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Model.Commands.Refactor
{
	internal class CreateCommand : CommandModelRefactor
	{
		private readonly IProjectSwitcher _switcher;
		private readonly IEditorStateChanger _stateChanger;
		private readonly IModelFactory _factory;

		public CreateCommand(IProjectSwitcher switcher, IEditorStateChanger stateChanger, IModelFactory factory)
		{
			_switcher = switcher;
			_stateChanger = stateChanger;
			_factory = factory;
		}
		
		internal override async Task Execute(IEditorState state)
		{
			var project = await state.GetLoadService().CreateAsync();
			if (project is not null)
			{
				_switcher.ResetProject(project);
			}
		}

		internal override void AfterExecute(IEditorState state)
		{
			var project = _switcher.GetProject();
			if (project is not null)
			{
				var projectFactory = _factory.ResolveProjectModelFactory(project);
				var next = projectFactory.ResolveNewEditorStateAsTransient();
				_stateChanger.ChangeState(next);
			}
		}
	}
}
