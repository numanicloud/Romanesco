using Romanesco.Model.ProjectComponents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Romanesco.Model.Services
{
    class NullLoadService : IProjectLoadService
    {
        public Project Create(ObjectInterpreter interpreter)
        {
            return null;
        }

        public Project Open()
        {
            return null;
        }
    }
}
