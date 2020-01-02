using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services
{
    class NullHistoryService : IProjectHistoryService
    {
        public void Redo()
        {
        }

        public void Undo()
        {
        }
    }
}
