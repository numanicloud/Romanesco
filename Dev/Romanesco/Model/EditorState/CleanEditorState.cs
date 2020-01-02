using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;

namespace Romanesco.Model.EditorState
{
    class CleanEditorState : EditorState
    {
        private readonly Project project;

        public override bool CanSave => true;

        public CleanEditorState(Editor editor, Project project) : base(editor)
        {
            this.project = project;
        }

        public override IProjectLoadService GetLoadService() => new NullLoadService();

        public override IProjectSaveService GetSaveService() => new NullSaveService();

        public override IProjectHistoryService GetHistoryService() => new NullHistoryService();
    }
}
