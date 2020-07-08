using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Common.Model.ProjectComponent
{
	public class ProjectContext
	{
		public IProjectTypeExporter Exporter { get; }
		public Project Project { get; }

		public ProjectContext(Project project, IProjectTypeExporter exporter)
		{
			Project = project;
			Exporter = exporter;
		}
	}
}
