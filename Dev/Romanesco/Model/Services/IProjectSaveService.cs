using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Model.Services
{
    interface IProjectSaveService
    {
        void Save();
        void SaveAs();
        void Export();
    }
}
