using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.Services;

namespace Romanesco.Model.EditorComponents
{
    public static class EditorFactory
    {
        public static IEditorFacade CreateEditor(
            IStateFactoryProvider factoryProvider,
            IProjectSettingProvider settingProvider)
        {
            return new Editor(factoryProvider, settingProvider);
        }
    }
}
