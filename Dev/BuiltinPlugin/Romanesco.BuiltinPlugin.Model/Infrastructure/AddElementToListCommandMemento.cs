using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.BuiltinPlugin.Model.Infrastructure
{
    public class AddElementToListCommandMemento : ICommandMemento
    {
        private readonly ListState list;
        private readonly IFieldState state;
        private readonly int index;

        public string CommandName => $"リスト {list.Title.Value} に要素を追加";

        public AddElementToListCommandMemento(ListState list, IFieldState state, int index)
        {
            this.list = list;
            this.state = state;
            this.index = index;
        }

        public void Redo()
        {
            list.Insert(state, index);
        }

        public void Undo()
        {
            list.RemoveAt(index);
        }
    }
}
