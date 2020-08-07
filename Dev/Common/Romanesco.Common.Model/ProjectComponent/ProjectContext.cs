using System;
using System.Linq;
using System.Reactive.Linq;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Common.Model.ProjectComponent
{
	public class ProjectContext
	{
		public IProjectTypeExporter Exporter { get; }
		public IProject Project { get; }

		public ProjectContext(IProject project, IProjectTypeExporter exporter)
		{
			Project = project;
			Exporter = exporter;
		}
		
		public IDisposable ObserveEdit(Action action)
		{
			return Project.Root.States
				.Select(x => x.OnEdited)
				.Merge()
				.Subscribe(x => action());
		}
	}
}
