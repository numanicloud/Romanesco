using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Reflections;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.Services;
using Romanesco.ViewModel.Editor;
using Romanesco.ViewModel.Infrastructure;

namespace Romanesco.Infrastructure
{
	class ModelRequirementFactory : Model.Infrastructure.IModelRequirementFactory
	{
		private readonly IOpenHostFactory host;
		public IOpenViewModelFactory? ViewModel { get; set; }

		public ModelRequirementFactory(IOpenHostFactory host)
		{
			this.host = host;
		}

		public IProjectSettingProvider ResolveProjectSettingProvider() => ViewModel?.ResolveProjectSettingProvider() ?? throw new InvalidOperationException($"{nameof(ViewModel)}プロパティを初期化してください。");

		public IDataAssemblyRepository ResolveDataAssemblyRepository() => host.ResolveDataAssemblyRepository();

		public CommandHistory ResolveCommandHistory() => host.ResolveCommandHistory();
	}

	class ViewModelRequirementFactory : ViewModel.Infrastructure.IViewModelRequirement
	{
		private readonly IOpenModelFactory model;

		public ViewModelRequirementFactory(IOpenModelFactory model)
		{
			this.model = model;
		}

		public IEditorFacade ResolveEditorFacade() => model.ResolveEditorFacade();
	}

	class ViewRequirementFactory : View.Infrastructure.IViewRequirementFactory
	{
		private readonly IOpenViewModelFactory viewModel;

		public ViewRequirementFactory(IOpenViewModelFactory viewModel)
		{
			this.viewModel = viewModel;
		}

		public IEditorViewModel ResolveEditorViewModel() => viewModel.ResolveEditorViewModel();
	}
}
