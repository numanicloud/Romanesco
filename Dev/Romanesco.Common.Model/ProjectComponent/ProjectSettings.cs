using System;
using System.Reflection;

namespace Romanesco.Model.ProjectComponents
{
    public class ProjectSettings
    {
        public Assembly Assembly { get; }
        public Type ProjectType { get; }
        public Type ExporterType { get; set; }

        public ProjectSettings(Assembly assembly, Type projectType, Type exporterType)
        {
            Assembly = assembly;
            ProjectType = projectType;
            ExporterType = exporterType;
        }
    }
}
