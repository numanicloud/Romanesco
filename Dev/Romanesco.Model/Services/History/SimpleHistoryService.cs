using Romanesco.Common.Model.Basics;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.Model.Services.History
{
    class SimpleHistoryService : IProjectHistoryService
    {
		private readonly ProjectContext context;

		public SimpleHistoryService(ProjectContext context)
        {
			this.context = context;
		}

        public bool CanUndo => context.CommandHistory.CanUndo;

        public bool CanRedo => context.CommandHistory.CanRedo;

        public void Redo()
        {
            context.CommandHistory.Redo();
        }

        public void Undo()
        {
            context.CommandHistory.Undo();
        }
    }
}
