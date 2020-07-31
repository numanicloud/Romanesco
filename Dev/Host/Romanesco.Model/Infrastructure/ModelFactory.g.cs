﻿// <autogenerated />
#nullable enable
using System;
using System.Collections.Generic;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Save;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Services.Serialize;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Model.Infrastructure
{
	internal partial class ModelFactory : IModelFactory
		, IDisposable
	{
		public IModelRequirementFactory Requirement { get; }
		public IPluginFactory Plugin { get; }

		private EditorStateChanger2? _ResolveEditorStateChanger2Cache;
		private WindowsLoadService? _ResolveProjectLoadServiceCache;
		private SimpleHistoryService? _ResolveProjectHistoryServiceCache;
		private ProjectSaveServiceFactory? _ResolveProjectSaveServiceFactoryCache;
		private ProjectModelFactory? _ResolveProjectModelFactoryCache;
		private Editor? _ResolveEditorFacadeCache;
		private NewtonsoftStateSerializer? _ResolveStateSerializerCache;
		private NewtonsoftStateDeserializer? _ResolveStateDeserializerCache;
		private ObjectInterpreter? _ResolveObjectInterpreterCache;
		private ObjectInterpreter? _ResolveIObjectInterpreterCache;

		public ModelFactory(IModelRequirementFactory requirement, IPluginFactory plugin)
		{
			Requirement = requirement;
			Plugin = plugin;
		}

		public EditorStateChanger2 ResolveEditorStateChanger2()
		{
			return _ResolveEditorStateChanger2Cache ??= new EditorStateChanger2(this);
		}

		public EmptyEditorState ResolveEmptyEditorStateAsTransient()
		{
			return new EmptyEditorState(ResolveProjectLoadService(), this, ResolveEditorStateChanger2());
		}

		public IProjectLoadService ResolveProjectLoadService()
		{
			return _ResolveProjectLoadServiceCache ??= new WindowsLoadService(Requirement.ResolveProjectSettingProvider(), ResolveStateDeserializer(), Requirement.ResolveDataAssemblyRepository(), this, ResolveObjectInterpreter());
		}

		public IProjectHistoryService ResolveProjectHistoryService()
		{
			return _ResolveProjectHistoryServiceCache ??= new SimpleHistoryService(Requirement.ResolveCommandHistory());
		}

		public ProjectSaveServiceFactory ResolveProjectSaveServiceFactory()
		{
			return _ResolveProjectSaveServiceFactoryCache ??= new ProjectSaveServiceFactory(ResolveStateSerializer());
		}

		public IProjectModelFactory ResolveProjectModelFactory(ProjectContext projectContext)
		{
			return _ResolveProjectModelFactoryCache ??= new ProjectModelFactory(projectContext, this, Requirement, Plugin);
		}

		public IEditorFacade ResolveEditorFacade()
		{
			return _ResolveEditorFacadeCache ??= new Editor(ResolveEditorStateChanger2());
		}

		public IStateSerializer ResolveStateSerializer()
		{
			return _ResolveStateSerializerCache ??= new NewtonsoftStateSerializer();
		}

		public IStateDeserializer ResolveStateDeserializer()
		{
			return _ResolveStateDeserializerCache ??= new NewtonsoftStateDeserializer();
		}

		public ProjectSettingsEditor ResolveProjectSettingsEditorAsTransient()
		{
			return new ProjectSettingsEditor(Requirement.ResolveDataAssemblyRepository());
		}

		public ObjectInterpreter ResolveObjectInterpreter()
		{
			return _ResolveObjectInterpreterCache ??= new ObjectInterpreter(Plugin.ResolveStateFactories());
		}

		public IObjectInterpreter ResolveIObjectInterpreter()
		{
			return _ResolveIObjectInterpreterCache ??= new ObjectInterpreter(Plugin.ResolveStateFactories());
		}

		public void Dispose()
		{
			_ResolveProjectModelFactoryCache?.Dispose();
			_ResolveEditorFacadeCache?.Dispose();
		}
	}
}