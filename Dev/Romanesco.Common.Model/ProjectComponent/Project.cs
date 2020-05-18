using Romanesco.Annotations;
using Romanesco.Common.Model.Basics;
using System.Linq;
using System.Reflection;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.Common.Model.ProjectComponents
{
    public class ProjectDependency
    {
        public Project Project { get; }
        public string Path { get; }

        public ProjectDependency(Project project, string path)
        {
            Project = project;
            Path = path;
        }
    }

    public class Project
    {
        public ProjectSettings Settings { get; }
        public StateRoot Root { get; }
        public string? DefaultSavePath { get; set; }
        public ProjectDependency[] DependencyProjects { get; }

        public Project(ProjectSettings settings, StateRoot root, ProjectDependency[] dependencyProjects)
        {
            Settings = settings;
            Root = root;
            DependencyProjects = dependencyProjects;
        }
    }
}
