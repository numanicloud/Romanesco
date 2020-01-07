using Romanesco.Common.Model.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace Romanesco.Common.Model.Basics
{
    public class CommandHistory
    {
        private readonly Stack<ICommandMemento> undos = new Stack<ICommandMemento>();
        private readonly Stack<ICommandMemento> redos = new Stack<ICommandMemento>();

        public bool CanUndo => undos.Count != 0;
        public bool CanRedo => redos.Count != 0;
        public IEnumerable<ICommandMemento> Undos => undos;
        public IEnumerable<ICommandMemento> Redos => redos;
        public bool IsOperating { get; private set; } = false;

        public void PushMemento(ICommandMemento memento)
        {
            undos.Push(memento);
            redos.Clear();
            Debug.Print($"Added {memento.CommandName}");
        }

        public void Undo()
        {
            if (undos.Count == 0)
            {
                return;
            }

            IsOperating = true;

            var memento = undos.Pop();
            redos.Push(memento);
            memento.Undo();

            IsOperating = false;

            Debug.Print($"Undoed {memento.CommandName}");
        }

        public void Redo()
        {
            if (redos.Count == 0)
            {
                return;
            }

            IsOperating = true;

            var memento = redos.Pop();
            undos.Push(memento);
            memento.Redo();

            IsOperating = false;

            Debug.Print($"Redoed {memento.CommandName}");
        }
    }
}
