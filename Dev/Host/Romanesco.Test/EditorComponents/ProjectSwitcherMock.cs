using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.Test.EditorComponents
{
	internal class ProjectSwitcherMock : IProjectSwitcher
	{
		private readonly ReactiveProperty<IProjectContext?> _projectContext = new();

		public IReadOnlyReactiveProperty<IProjectContext?> ProjectStream => _projectContext;

		public IProjectContext? GetProject() => _projectContext.Value;

		public void ResetProject(IProjectContext project) => _projectContext.Value = project;

		public Subject<Unit> BeforeResetProject { get; } = new();
	}
}
