using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Romanesco.Model.Services
{
    class NullSaveService : IProjectSaveService
    {
        public void Export()
        {
        }

        public Task SaveAsAsync()
        {
            return Task.CompletedTask;
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }
}
