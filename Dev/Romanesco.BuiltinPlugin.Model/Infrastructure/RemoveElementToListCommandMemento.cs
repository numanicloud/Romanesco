using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.BuiltinPlugin.Model.Infrastructure
{
    class RemoveElementToListCommandMemento : ICommandMemento
    {
        private readonly ListState list;
        private readonly IFieldState state;
        private readonly int index;

        public string CommandName => $"リスト {list.Title.Value} の {index} 要素目を削除";

        public RemoveElementToListCommandMemento(ListState list, IFieldState state, int index)
        {
            this.list = list;
            this.state = state;
            this.index = index;
        }

        public void Redo()
        {
            list.RemoveAt(index);
        }

        public void Undo()
        {
            list.Insert(state, index);
        }
    }
}
