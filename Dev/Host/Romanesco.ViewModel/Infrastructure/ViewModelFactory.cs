using Imfact.Annotations;
using Romanesco.Model.Services;
using Romanesco.ViewModel.Editor;
using Romanesco.ViewModel.Project;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Infrastructure;

[Factory]
internal partial class ViewModelFactory : IViewModelFactory
{
	public IViewModelRequirement Requirement { get; }
	public IPluginFactory Plugin { get; }

	[Resolution(typeof(EditorViewModel)), Cache]
	public partial IEditorViewModel ResolveEditorViewModel();

	[Resolution(typeof(VmProjectSettingsProvider)), Cache]
	public partial IProjectSettingProvider ResolveProjectSettingProvider();

	[Resolution(typeof(ViewModelInterpreter))]
	public partial IViewModelInterpreter ResolveViewModelInterpreter();
}