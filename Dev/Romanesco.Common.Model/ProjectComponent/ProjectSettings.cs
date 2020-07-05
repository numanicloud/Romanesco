using System;
using System.Reflection;

namespace Romanesco.Common.Model.ProjectComponent
{
    public class ProjectSettings
    {
        public Assembly Assembly { get; }
        public Type ProjectType { get; }
        public Type ExporterType { get; set; }
        public string[] DependencyProjects { get; set; }

        public ProjectSettings(Assembly assembly, Type projectType, Type exporterType,
            string[] dependencyProjects)
        {
            Assembly = assembly;
            ProjectType = projectType;
            ExporterType = exporterType;
            DependencyProjects = dependencyProjects;
        }
    }
}
