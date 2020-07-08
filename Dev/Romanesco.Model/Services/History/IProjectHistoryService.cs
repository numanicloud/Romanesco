namespace Romanesco.Model.Services.History
{
	internal interface IProjectHistoryService
    {
        public bool CanUndo { get; }
        public bool CanRedo { get; }
        void Redo();
        void Undo();
    }
}
