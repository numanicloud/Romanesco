using System;
using System.Reflection;

namespace Romanesco.Model.ProjectComponents
{
    public class ProjectSettings
    {
        public Assembly ProjectAssembly { get; }
        public Type ProjectType { get; }
        public Assembly Assembly { get; }

        public ProjectSettings(Assembly assembly, Type projectType)
        {
            Assembly = assembly;
            ProjectType = projectType;
        }
    }
}
