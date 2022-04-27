using System.Threading.Tasks;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.States;

namespace Romanesco.Model.Commands
{
	internal class SaveRomanescoCommand : RomanescoCommand
	{
		private readonly IProjectSwitcher _switcher;
		private readonly IEditorStateChanger _stateChanger;
		private readonly IModelFactory _factory;

		public SaveRomanescoCommand(IProjectSwitcher switcher, IEditorStateChanger stateChanger, IModelFactory factory)
		{
			_switcher = switcher;
			_stateChanger = stateChanger;
			_factory = factory;
		}

		internal override async Task Execute(IEditorState state)
		{
			await state.GetSaveService().SaveAsync();
		}

		internal override void AfterExecute(IEditorState state)
		{
			var project = _switcher.GetProject();
			if (project is not null)
			{
				var projectFactory = _factory.ResolveProjectModelFactoryAsTransient(project);
				_stateChanger.ChangeState(projectFactory.ResolveCleanEditorStateAsTransient());
			}
		}
	}
}
