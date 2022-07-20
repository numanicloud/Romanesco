using Imfact.Annotations;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Common.Model.Reflections;
using Romanesco.Model.Commands;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Model.Services.Serialize;
using Romanesco.Model.States;

namespace Romanesco.Model.Infrastructure;

[Factory]
internal partial class ImfactModelFactory : IModelFactory
{
	public IModelRequirementFactory Requirement { get; }
	public IPluginFactory Plugin { get; }

	[Cache] public partial ProjectSettingsEditor ResolveProjectSettingsEditorAsTransient();
	[Cache] public partial ObjectInterpreter ResolveObjectInterpreter();
	[Cache] public partial Editor ResolveEditor();
	[Cache] public partial ProjectSaveServiceFactory ResolveProjectSaveServiceFactory();
	[Cache] public partial CommandContext ResolveCommandContext();
	public partial EmptyEditorState ResolveEmptyEditorStateAsTransient();

	public IEditorFacade ResolveEditorFacade() => ResolveEditor();
	public IObjectInterpreter ResolveIObjectInterpreter() => ResolveObjectInterpreter();
	public IEditorState ResolveEditorState() => ResolveEmptyEditorStateAsTransient();

	[Resolution(typeof(NewtonsoftStateSerializer)), Cache]
	public partial IStateSerializer ResolveStateSerializer();

	[Resolution(typeof(NewtonsoftStateDeserializer)), Cache]
	public partial IStateDeserializer ResolveStateDeserializer();

	[Resolution(typeof(ProjectSwitcher)), Cache]
	public partial IProjectSwitcher ResolveProjectSwitcher();

	[Resolution(typeof(StorageCloneService)), Cache]
	public partial IStorageCloneService ResolveStorageCloneService();

	[Resolution(typeof(EditorStateChanger)), Cache]
	public partial IEditorStateChanger ResolveEditorStateChanger();

	[Resolution(typeof(WindowsLoadService)), Cache]
	public partial IProjectLoadService ResolveProjectLoadService();

	[Resolution(typeof(SimpleHistoryService)), Cache]
	public partial IProjectHistoryService ResolveProjectHistoryService();

	[Resolution(typeof(DataAssemblyRepository)), Cache]
	public partial IDataAssemblyRepository ResolveDataAssemblyRepository();

	[Resolution(typeof(ImfactProjectModelFactory))]
	public partial IProjectModelFactory ResolveProjectModelFactoryAsTransient
		(IProjectContext context);
}