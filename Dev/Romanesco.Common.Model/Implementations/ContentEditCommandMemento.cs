using Romanesco.Common.Model.Interfaces;
using System;

namespace Romanesco.Common.Model.Implementations
{
    public class ContentEditCommandMemento : ICommandMemento
    {
        private readonly Action<object?> setter;
        private readonly object? prev;
        private readonly object? current;

        public string CommandName => $"メンバーに {current} を設定";

        public ContentEditCommandMemento(Action<object?> setter, object? prev, object? current)
        {
            this.setter = setter;
            this.prev = prev;
            this.current = current;
        }

        public void Redo()
        {
            setter(current);
        }

        public void Undo()
        {
            setter(prev);
        }
    }
}
