using System.Collections.Generic;
using Deptorygen.Annotations;
using Romanesco.Common.Model.Basics;
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

namespace Romanesco.Model.Infrastructure
{
	public interface IOpenModelFactory
	{
		[Delegation("ResolveEditor")]
		IEditorFacade ResolveEditorFacade();

		[Resolution(typeof(NewtonsoftStateSerializer))]
		IStateSerializer ResolveStateSerializer();
		[Resolution(typeof(NewtonsoftStateDeserializer))]
		IStateDeserializer ResolveStateDeserializer();
		ProjectSettingsEditor ResolveProjectSettingsEditorAsTransient();
		ObjectInterpreter ResolveObjectInterpreter();
		[Resolution(typeof(ObjectInterpreter))]
		IObjectInterpreter ResolveIObjectInterpreter();
		[Resolution(typeof(ProjectSwitcher))]
		IProjectSwitcher ResolveProjectSwitcher();
		[Resolution(typeof(StorageCloneService))]
		IStorageCloneService ResolveStorageCloneService();
	}

	[Factory]
	public interface IModelRequirementFactory
	{
		IProjectSettingProvider ResolveProjectSettingProvider();
		IDataAssemblyRepository ResolveDataAssemblyRepository();
		CommandHistory ResolveCommandHistory();
	}

	[Factory]
	public interface IPluginFactory
	{
		IEnumerable<IStateFactory> ResolveStateFactories();
		ILoadingStateProvider ResolveLoadingStateProvider();
	}

	// TODO: IEditorFacade と IEditorStateRepository で同じインスタンスを返したい
	[Factory]
	internal interface IModelFactory : IOpenModelFactory
	{
		Editor ResolveEditor();

		IModelRequirementFactory Requirement { get; }
		IPluginFactory Plugin { get; }

		[Resolution(typeof(EditorStateChanger))]
		IEditorStateChanger ResolveEditorStateChanger();
		[Resolution(typeof(EmptyEditorState))]
		IEditorState ResolveEditorState();

		EmptyEditorState ResolveEmptyEditorStateAsTransient();
		[Resolution(typeof(WindowsLoadService))]
		IProjectLoadService ResolveProjectLoadService();
		[Resolution(typeof(SimpleHistoryService))]
		IProjectHistoryService ResolveProjectHistoryService();
		ProjectSaveServiceFactory ResolveProjectSaveServiceFactory();

		[Resolution(typeof(ProjectModelFactory))]
		IProjectModelFactory ResolveProjectModelFactoryAsTransient(IProjectContext projectContext);
		
		CommandContext ResolveCommandContext();
	}
}
