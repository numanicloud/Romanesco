using Romanesco.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services
{
    interface IProjectSaveService
    {
        void Save(Project project);
        void SaveAs(Project project);
    }
}
