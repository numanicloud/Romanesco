using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponents;

namespace Romanesco.Common.Model.Basics
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
