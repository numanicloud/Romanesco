using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common.Interfaces
{
    public interface ICommandMemento
    {
        string CommandName { get; }
        void Redo();
        void Undo();
    }
}
