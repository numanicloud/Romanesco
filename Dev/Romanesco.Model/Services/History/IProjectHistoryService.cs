namespace Romanesco.Model.Services.History
{
    interface IProjectHistoryService
    {
        void Redo();
        void Undo();
    }
}
