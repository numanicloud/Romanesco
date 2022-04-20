using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Model.Commands.Refactor
{
	internal class SaveAsCommand : CommandModelRefactor
	{
		private readonly IProjectSwitcher _switcher;
		private readonly IEditorStateChanger _stateChanger;
		private readonly IModelFactory _factory;

		public SaveAsCommand
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
				var projectFactory = _factory.ResolveProjectModelFactory(project);
				var next = projectFactory.ResolveCleanEditorStateAsTransient();
				_stateChanger.ChangeState(next);
			}
		}
	}
}
