using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Common.Model.Basics;

namespace Romanesco.Common.Model.ProjectComponent
{
	public interface IProject
	{
		ProjectSettings Settings { get; }
		StateRoot Root { get; }
		string? DefaultSavePath { get; set; }
		ProjectDependency[] DependencyProjects { get; }
	}
}
