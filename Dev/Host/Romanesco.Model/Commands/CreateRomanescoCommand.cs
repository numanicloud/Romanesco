using System.Threading.Tasks;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.States;

namespace Romanesco.Model.Commands
{
	internal class CreateRomanescoCommand : RomanescoCommand
	{
		private readonly IProjectSwitcher _switcher;
		private readonly IEditorStateChanger _stateChanger;
		private readonly IModelFactory _factory;

		public CreateRomanescoCommand(IProjectSwitcher switcher, IEditorStateChanger stateChanger, IModelFactory factory)
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
				var projectFactory = _factory.ResolveProjectModelFactoryAsTransient(project);
				var next = projectFactory.ResolveNewEditorStateAsTransient();
				_stateChanger.ChangeState(next);
			}
		}
	}
}
