using Romanesco.Model.ProjectComponents;

namespace Romanesco.Model.Services
{
    class SimpleHistoryService : IProjectHistoryService
    {
        private readonly Project project;

        public SimpleHistoryService(Project project)
        {
            this.project = project;
        }

        public void Redo()
        {
            //project.Context.CommandHistory.Redo();
        }

        public void Undo()
        {
            //project.Context.CommandHistory.Undo();
        }
    }
}
