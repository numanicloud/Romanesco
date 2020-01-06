using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Romanesco.Model.Services
{
    interface IProjectSaveService
    {
        Task SaveAsync();
        Task SaveAsAsync();
        void Export();
    }
}
