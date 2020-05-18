using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.ProjectComponents;
using System;
using System.Collections.Generic;

namespace Romanesco.Common.Model.Basics
{
	public class ProjectContext
	{
		private readonly ProjectContextCrawler crawler;

		public CommandHistory CommandHistory => crawler.CommandHistory;
		public IProjectTypeExporter Exporter { get; }
		public Project Project { get; }

		public ProjectContext(Project project, ProjectContextCrawler crawler, IProjectTypeExporter exporter)
		{
			Project = project;
			this.crawler = crawler;
			Exporter = exporter;
		}
	}
}
