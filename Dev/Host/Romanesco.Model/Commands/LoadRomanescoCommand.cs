using System.Threading.Tasks;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.States;

namespace Romanesco.Model.Commands
{
	internal class LoadRomanescoCommand : RomanescoCommand
	{
		private readonly IProjectSwitcher _projectSwitcher;
		private readonly IEditorStateChanger _stateChanger;
		private readonly IModelFactory _factory;

		public LoadRomanescoCommand(IProjectSwitcher projectSwitcher,
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
				var projectFactory = _factory.ResolveProjectModelFactoryAsTransient(project);
				var next = projectFactory.ResolveCleanEditorStateAsTransient();
				_stateChanger.ChangeState(next);
			}
		}
	}
}
