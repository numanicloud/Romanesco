using System;
using Reactive.Bindings;
using Romanesco.Common.Model.ProjectComponent;

namespace Romanesco.Model.Commands.Refactor;

public interface IProjectSwitcher
{
	IReadOnlyReactiveProperty<IProjectContext?> ProjectStream { get; }
	IProjectContext? GetProject();
	void ResetProject(IProjectContext project);
}
