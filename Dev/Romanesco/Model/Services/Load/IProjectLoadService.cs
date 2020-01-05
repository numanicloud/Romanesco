using Romanesco.Model.ProjectComponents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Romanesco.Model.Services
{
    interface IProjectLoadService
    {
        Project Create();
        Task<Project> OpenAsync();
    }
}
