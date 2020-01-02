using Romanesco.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common.Utility
{
    public class ProjectContext
    {
        public CommandHistory CommandHistory { get; } = new CommandHistory();
    }
}
