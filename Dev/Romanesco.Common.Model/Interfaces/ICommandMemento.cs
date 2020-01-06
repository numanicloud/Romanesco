namespace Romanesco.Common.Model.Interfaces
{
    public interface ICommandMemento
    {
        string CommandName { get; }
        void Redo();
        void Undo();
    }
}
