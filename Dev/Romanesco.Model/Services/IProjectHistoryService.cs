using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services
{
    interface IProjectHistoryService
    {
        void Redo();
        void Undo();
    }
}
