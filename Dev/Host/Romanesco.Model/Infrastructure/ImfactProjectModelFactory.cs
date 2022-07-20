using Imfact.Annotations;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Model.Services.Serialize;
using Romanesco.Model.States;

namespace Romanesco.Model.Infrastructure;

// FactoryからFactoryを生成することができないみたい
[Factory]
internal partial class ImfactProjectModelFactory : IProjectModelFactory
{
	public IOpenModelFactory OpenModel => Model;
	public IModelFactory Model { get; }

	public partial NewEditorState ResolveNewEditorStateAsTransient();

	public partial CleanEditorState ResolveCleanEditorStateAsTransient();

	public partial DirtyEditorState ResolveDirtyEditorStateAsTransient();

	[Resolution(typeof(WindowsSaveService))]
	public partial IProjectSaveService ResolveSaveService();

	#region Delegation-IModelFactory
	public IEditorFacade ResolveEditorFacade()
		=> Model.ResolveEditorFacade();

	public IStateSerializer ResolveStateSerializer()
		=> Model.ResolveStateSerializer();

	public IStateDeserializer ResolveStateDeserializer()
		=> Model.ResolveStateDeserializer();

	public ProjectSettingsEditor ResolveProjectSettingsEditorAsTransient()
		=> Model.ResolveProjectSettingsEditorAsTransient();

	public ObjectInterpreter ResolveObjectInterpreter()
		=> Model.ResolveObjectInterpreter();

	public IObjectInterpreter ResolveIObjectInterpreter()
		=> Model.ResolveIObjectInterpreter();

	public IProjectSwitcher ResolveProjectSwitcher()
		=> Model.ResolveProjectSwitcher();

	public IStorageCloneService ResolveStorageCloneService()
		=> Model.ResolveStorageCloneService();

	public Editor ResolveEditor()
		=> Model.ResolveEditor();

	public IModelRequirementFactory Requirement => Model.Requirement;

	public IPluginFactory Plugin => Model.Plugin;

	public IEditorStateChanger ResolveEditorStateChanger()
		=> Model.ResolveEditorStateChanger();

	public IEditorState ResolveEditorState()
		=> Model.ResolveEditorState();

	public EmptyEditorState ResolveEmptyEditorStateAsTransient()
		=> Model.ResolveEmptyEditorStateAsTransient();

	public IProjectLoadService ResolveProjectLoadService()
		=> Model.ResolveProjectLoadService();

	public IProjectHistoryService ResolveProjectHistoryService()
		=> Model.ResolveProjectHistoryService();

	public ProjectSaveServiceFactory ResolveProjectSaveServiceFactory()
		=> Model.ResolveProjectSaveServiceFactory();

	public IProjectModelFactory ResolveProjectModelFactoryAsTransient(IProjectContext context)
		=> Model.ResolveProjectModelFactoryAsTransient(context);

	public CommandContext ResolveCommandContext()
		=> Model.ResolveCommandContext();
	#endregion
}