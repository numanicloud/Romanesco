﻿// <autogenerated />
#nullable enable
using System;
using System.Collections.Generic;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Save;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.History;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Services.Serialize;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Model.Infrastructure
{
	internal partial class ProjectModelFactory : IProjectModelFactory
		, IDisposable
	{
		private readonly ProjectContext _projectContext;

		public IModelFactory Model { get; }
		public IModelRequirementFactory Requirement { get; }
		public IPluginFactory Plugin { get; }

		private WindowsSaveService? _ResolveProjectSaveServiceCache;

		public ProjectModelFactory(ProjectContext projectContext, IModelFactory model, IModelRequirementFactory requirement, IPluginFactory plugin)
		{
			_projectContext = projectContext;
			Model = model;
			Requirement = requirement;
			Plugin = plugin;
		}

		public NewEditorState ResolveNewEditorStateAsTransient()
		{
			return new NewEditorState(Model.ResolveProjectLoadService(), Model.ResolveProjectHistoryService(), ResolveProjectSaveService(), this, Model.ResolveEditorStateChanger());
		}

		public CleanEditorState ResolveCleanEditorStateAsTransient()
		{
			return new CleanEditorState(Model.ResolveProjectLoadService(), Model.ResolveProjectHistoryService(), ResolveProjectSaveService(), _projectContext, this, Model.ResolveEditorStateChanger());
		}

		public DirtyEditorState ResolveDirtyEditorStateAsTransient()
		{
			return new DirtyEditorState(Model.ResolveProjectLoadService(), Model.ResolveProjectHistoryService(), ResolveProjectSaveService(), _projectContext, this, Model.ResolveEditorStateChanger());
		}

		public IProjectSaveService ResolveProjectSaveService()
		{
			return _ResolveProjectSaveServiceCache ??= new WindowsSaveService(Model.ResolveStateSerializer(), _projectContext);
		}

		public IEditorStateChanger ResolveEditorStateChanger()
		{
			return Model.ResolveEditorStateChanger();
		}

		public EmptyEditorState ResolveEmptyEditorStateAsTransient()
		{
			return Model.ResolveEmptyEditorStateAsTransient();
		}

		public IProjectLoadService ResolveProjectLoadService()
		{
			return Model.ResolveProjectLoadService();
		}

		public IProjectHistoryService ResolveProjectHistoryService()
		{
			return Model.ResolveProjectHistoryService();
		}

		public ProjectSaveServiceFactory ResolveProjectSaveServiceFactory()
		{
			return Model.ResolveProjectSaveServiceFactory();
		}

		public IProjectModelFactory ResolveProjectModelFactory(ProjectContext projectContext)
		{
			return Model.ResolveProjectModelFactory(_projectContext);
		}

		public IEditorFacade ResolveEditorFacade()
		{
			return Model.ResolveEditorFacade();
		}

		public IStateSerializer ResolveStateSerializer()
		{
			return Model.ResolveStateSerializer();
		}

		public IStateDeserializer ResolveStateDeserializer()
		{
			return Model.ResolveStateDeserializer();
		}

		public ProjectSettingsEditor ResolveProjectSettingsEditorAsTransient()
		{
			return Model.ResolveProjectSettingsEditorAsTransient();
		}

		public ObjectInterpreter ResolveObjectInterpreter()
		{
			return Model.ResolveObjectInterpreter();
		}

		public IObjectInterpreter ResolveIObjectInterpreter()
		{
			return Model.ResolveIObjectInterpreter();
		}

		public void Dispose()
		{
		}
	}
}