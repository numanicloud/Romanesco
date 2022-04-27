using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using Reactive.Bindings;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.Model
{
	public interface IEditorFacade
    {
        List<IDisposable> Disposables { get; }
        CommandContext CommandContext { get; }
        IProjectSwitcher ProjectSwitcher { get; }
		ReactiveProperty<IFieldState[]> Roots { get; }
	}

	internal class ProjectSwitcher : IProjectSwitcher
	{
		private readonly ReactiveProperty<IProjectContext?> _project = new();

		public IReadOnlyReactiveProperty<IProjectContext?> ProjectStream => _project;

		public Subject<Unit> BeforeResetProject { get; } = new();

		public IProjectContext? GetProject()
		{
			return _project.Value;
		}

		public void ResetProject(IProjectContext project)
		{
			_project.Value = project;
		}
	}
}
