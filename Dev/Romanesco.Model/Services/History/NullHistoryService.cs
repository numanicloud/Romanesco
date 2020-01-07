namespace Romanesco.Model.Services.History
{
    class NullHistoryService : IProjectHistoryService
    {
        public bool CanUndo => false;

        public bool CanRedo => false;

        public void Redo()
        {
        }

        public void Undo()
        {
        }
    }
}
