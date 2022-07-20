using System.Collections.Generic;
using Imfact.Annotations;
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
	[Factory]
	public interface IOpenModelFactory
	{
		ProjectSettingsEditor ResolveProjectSettingsEditorAsTransient();
		ObjectInterpreter ResolveObjectInterpreter();

		IEditorFacade ResolveEditorFacade();
		IStateSerializer ResolveStateSerializer();
		IStateDeserializer ResolveStateDeserializer();
		IObjectInterpreter ResolveIObjectInterpreter();
		IProjectSwitcher ResolveProjectSwitcher();
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
		IModelRequirementFactory Requirement { get; }
		IPluginFactory Plugin { get; }

		Editor ResolveEditor();
		EmptyEditorState ResolveEmptyEditorStateAsTransient();
		ProjectSaveServiceFactory ResolveProjectSaveServiceFactory();
		CommandContext ResolveCommandContext();

		IEditorStateChanger ResolveEditorStateChanger();
		IEditorState ResolveEditorState();
		IProjectLoadService ResolveProjectLoadService();
		IProjectHistoryService ResolveProjectHistoryService();
		IProjectModelFactory ResolveProjectModelFactoryAsTransient(IProjectContext context);
	}
}
