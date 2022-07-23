using Imfact.Annotations;
using Romanesco.View.Entry;
using Romanesco.View.States;

namespace Romanesco.View.Infrastructure;

[Factory]
internal partial class ViewFactory : IViewFactory
{
	public IViewRequirementFactory Requirement { get; }
	public IPluginFactory Plugin { get; }

	[Resolution(typeof(MainDataContext)), Cache]
	public partial IEditorViewContext ResolveEditorViewContext();

	public partial ViewInterpreter ResolveViewInterpreter();
}