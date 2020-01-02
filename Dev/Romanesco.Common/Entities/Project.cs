using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common.Entities
{
    public class Project
    {
        public ProjectContext Context { get; }

        public Project(ProjectContext context)
        {
            Context = context;
        }
    }
}
