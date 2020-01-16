using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.Common.Model.Basics
{
    public class ProjectContext
    {
		private readonly ProjectContextCrawler crawler;

        public CommandHistory CommandHistory => crawler.CommandHistory;
        public IProjectTypeExporter Exporter { get; set; }
		public Project Project { get; }

        public ProjectContext(Project project, ProjectContextCrawler crawler)
        {
            Project = project;
			this.crawler = crawler;
		}
    }
}
