using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Common.Interfaces;
using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common.Utility
{
    public class ContentEditCommandMemento : ICommandMemento
    {   
        private readonly Action<object> setter;
        private readonly object prev;
        private readonly object current;

        public string CommandName => $"メンバーに {current} を設定";

        public ContentEditCommandMemento(Action<object> setter, object prev, object current)
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
