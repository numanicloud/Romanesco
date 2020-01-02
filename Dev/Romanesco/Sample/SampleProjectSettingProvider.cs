using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Romanesco.Sample
{
    class SampleProjectSettingProvider : IProjectSettingProvider
    {
        public ProjectSettings GetSettings()
        {
            var assembly = typeof(Project).Assembly;
            var projectType = typeof(Project);
            return new ProjectSettings(assembly, projectType);
        }
    }
}
