using System.Threading.Tasks;
using Romanesco.Model.Commands.Refactor;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Model.Commands
{
	internal class SaveAsRomanescoCommand : RomanescoCommand
	{
		private readonly IProjectSwitcher _switcher;
		private readonly IEditorStateChanger _stateChanger;
		private readonly IModelFactory _factory;

		public SaveAsRomanescoCommand
			(IProjectSwitcher switcher,
			IEditorStateChanger stateChanger,
			IModelFactory factory)
		{
			_switcher = switcher;
			_stateChanger = stateChanger;
			_factory = factory;
		}

		internal override async Task Execute(IEditorState state)
		{
			await state.GetSaveService().SaveAsAsync();
		}

		internal override void AfterExecute(IEditorState state)
		{
			var project = _switcher.GetProject();
			if (project is not null)
			{
				var projectFactory = _factory.ResolveProjectModelFactoryAsTransient(project);
				var next = projectFactory.ResolveCleanEditorStateAsTransient();
				_stateChanger.ChangeState(next);
			}
		}
	}
}
