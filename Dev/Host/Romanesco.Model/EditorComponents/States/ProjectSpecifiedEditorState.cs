using Romanesco.Model.Commands;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Model.EditorComponents.States
{
	abstract class ProjectSpecifiedEditorState : EditorState
	{
		private readonly IProjectModelFactory factory;

		protected ProjectSpecifiedEditorState(IProjectModelFactory factory, IEditorStateChanger editorStateChanger)
			: base(factory, editorStateChanger)
		{
			this.factory = factory;
		}

		public override void OnSave()
		{
			StateChanger.ChangeState(factory.ResolveCleanEditorStateAsTransient());
		}

		public override void OnSaveAs()
		{
			StateChanger.ChangeState(factory.ResolveCleanEditorStateAsTransient());
		}

		public override void OnEdit()
		{
			StateChanger.ChangeState(factory.ResolveDirtyEditorStateAsTransient());
		}
	}
}
