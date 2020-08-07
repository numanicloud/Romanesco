﻿// <autogenerated />
#nullable enable
using System;
using System.Collections.Generic;
using Romanesco.ViewModel.States;
using Romanesco.ViewModel.Editor;
using Romanesco.Model.Services;
using Romanesco.ViewModel.Project;

namespace Romanesco.ViewModel.Infrastructure
{
	internal partial class ViewModelFactory : IViewModelFactory
		, IDisposable
	{
		public IViewModelRequirement Requirement { get; }
		public IPluginFactory Plugin { get; }

		private ViewModelInterpreter? _ResolveViewModelInterpreterCache;
		private EditorViewModel? _ResolveEditorViewModelCache;
		private VmProjectSettingsProvider? _ResolveProjectSettingProviderCache;

		public ViewModelFactory(IViewModelRequirement requirement, IPluginFactory plugin)
		{
			Requirement = requirement;
			Plugin = plugin;
		}

		public ViewModelInterpreter ResolveViewModelInterpreter()
		{
			return _ResolveViewModelInterpreterCache ??= new ViewModelInterpreter(Plugin.ResolveStateViewModelFactories());
		}

		public IEditorViewModel ResolveEditorViewModel()
		{
			return _ResolveEditorViewModelCache ??= new EditorViewModel(Requirement.ResolveEditorFacade(), ResolveViewModelInterpreter());
		}

		public IProjectSettingProvider ResolveProjectSettingProvider()
		{
			return _ResolveProjectSettingProviderCache ??= new VmProjectSettingsProvider(this);
		}

		public void Dispose()
		{
			_ResolveEditorViewModelCache?.Dispose();
		}
	}
}