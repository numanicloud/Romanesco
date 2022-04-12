using Romanesco.Common.Model.Basics;

namespace Romanesco.Common.Model.ProjectComponent
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

    public class Project : IProject
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
