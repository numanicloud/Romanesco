using System;
using System.Reflection;

namespace Romanesco.Model.ProjectComponents
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
