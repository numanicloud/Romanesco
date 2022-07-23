using System.Collections.Generic;
using Imfact.Annotations;
using Romanesco.View.Entry;
using Romanesco.View.States;
using Romanesco.ViewModel.Editor;

namespace Romanesco.View.Infrastructure
{
	// 依存先として。静的に解決できる
	public interface IOpenViewFactory
	{
		IEditorViewContext ResolveEditorViewContext();
	}

	// 依存元として。静的に解決できる
	[Factory]
	public interface IViewRequirementFactory
	{
		IEditorViewModel ResolveEditorViewModel();
	}

	// 依存元として。静的に解決できない
	[Factory]
	public interface IPluginFactory
	{
		IEnumerable<Common.View.Interfaces.IViewFactory> ResolveViewFactories();

		IEnumerable<Common.View.Interfaces.IResourceDictionaryFactory> ResolveResourceDictionaryFactories();

		IEnumerable<Common.View.Interfaces.IRootViewFactory> ResolveRootViewFactories();
	}

	// 本体。publicでないものを含めて解決する
	[Factory]
	internal interface IViewFactory : IOpenViewFactory
	{
		IViewRequirementFactory Requirement { get; }
		IPluginFactory Plugin { get; }

		ViewInterpreter ResolveViewInterpreter();
	}
}
