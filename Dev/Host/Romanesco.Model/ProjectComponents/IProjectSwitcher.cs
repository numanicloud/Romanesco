using System;
using System.Reactive;
using System.Reactive.Subjects;
using Reactive.Bindings;
using Romanesco.Common.Model.ProjectComponent;

namespace Romanesco.Model.Commands.Refactor;

public interface IProjectSwitcher
{
	IReadOnlyReactiveProperty<IProjectContext?> ProjectStream { get; }
	IProjectContext? GetProject();
	void ResetProject(IProjectContext project);
	Subject<Unit> BeforeResetProject { get; }
}
