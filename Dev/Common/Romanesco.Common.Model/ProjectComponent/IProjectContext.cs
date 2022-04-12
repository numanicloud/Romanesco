using Romanesco.Common.Model.Interfaces;
using System;
using Romanesco.Common.Model.Basics;

namespace Romanesco.Common.Model.ProjectComponent
{
	public interface IProjectContext
	{
		IProjectTypeExporter Exporter { get; }
		IProject Project { get; }
		StateRoot StateRoot { get; }

		IDisposable ObserveEdit(Action action);
	}
}