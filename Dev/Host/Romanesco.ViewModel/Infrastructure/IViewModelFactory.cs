using System.Collections.Generic;
using Deptorygen.Annotations;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.Services;
using Romanesco.ViewModel.Editor;
using Romanesco.ViewModel.Project;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Infrastructure
{
	public interface IOpenViewModelFactory
	{
		[Resolution(typeof(EditorViewModel))]
		IEditorViewModel ResolveEditorViewModel();
		[Resolution(typeof(VmProjectSettingsProvider))]
		IProjectSettingProvider ResolveProjectSettingProvider();
	}

	[Factory]
	public interface IViewModelRequirement
	{
		IEditorFacade ResolveEditorFacade();
	}

	[Factory]
	public interface IPluginFactory
	{
		IEnumerable<IStateViewModelFactory> ResolveStateViewModelFactories();
	}

	[Factory]
	internal interface IViewModelFactory : IOpenViewModelFactory
	{
		IViewModelRequirement Requirement { get; }
		IPluginFactory Plugin { get; }

		ViewModelInterpreter ResolveViewModelInterpreter();
	}
}
