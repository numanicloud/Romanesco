using System.Threading.Tasks;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Model.Commands.Refactor
{
	internal class LoadCommand : CommandModelRefactor
	{
		private readonly IProjectSwitcher _projectSwitcher;
		private readonly IEditorStateChanger _stateChanger;
		private readonly IModelFactory _factory;

		public LoadCommand(IProjectSwitcher projectSwitcher,
			IEditorStateChanger stateChanger,
			IModelFactory factory)
		{
			_projectSwitcher = projectSwitcher;
			_stateChanger = stateChanger;
			_factory = factory;
		}

		internal override async Task Execute(IEditorState state)
		{
			var project = await state.GetLoadService().OpenAsync();
			if (project is not null)
			{
				_projectSwitcher.ResetProject(project);
			}
		}

		internal override void AfterExecute(IEditorState state)
		{
			var project = _projectSwitcher.GetProject();
			if (project is not null)
			{
				var projectFactory = _factory.ResolveProjectModelFactory(project);
				var next = projectFactory.ResolveCleanEditorStateAsTransient();
				_stateChanger.ChangeState(next);
			}
		}
	}
}
