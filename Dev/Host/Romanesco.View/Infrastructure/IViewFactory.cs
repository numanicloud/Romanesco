using System;
using System.Collections.Generic;
using System.Text;
using Deptorygen.Annotations;
using Romanesco.View.Entry;
using Romanesco.View.States;
using Romanesco.ViewModel.Editor;

namespace Romanesco.View.Infrastructure
{
	// 依存先として。静的に解決できる
	public interface IOpenViewFactory
	{
		[Resolution(typeof(MainDataContext))]
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
