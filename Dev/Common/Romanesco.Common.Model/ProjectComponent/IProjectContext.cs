using Romanesco.Common.Model.Interfaces;
using System;

namespace Romanesco.Common.Model.ProjectComponent
{
	public interface IProjectContext
	{
		IProjectTypeExporter Exporter { get; }
		IProject Project { get; }

		IDisposable ObserveEdit(Action action);
	}
}