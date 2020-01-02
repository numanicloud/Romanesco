using Romanesco.Model.ProjectComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services
{
    interface IProjectLoadService
    {
        Project Create(ObjectInterpreter interpreter);
        Project Open();
    }
}
