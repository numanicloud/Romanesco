using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;

namespace Romanesco.Model.ProjectComponents
{
	public class ProjectContextCrawler
	{
		public object[] InstancesDependsOn { get; set; } = new object[0];
		public CommandHistory CommandHistory { get; set; } = new CommandHistory();
	}
}