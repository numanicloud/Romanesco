using Romanesco.Common.Model.Basics;
using Romanesco.Extensibility;
using Romanesco.Sample;
using Romanesco.View;
using Romanesco.Model.EditorComponents;

namespace Romanesco
{
    class StartUp
    {
        public MainDataContext LoadMainDataContext()
        {
            var loader = new PluginLoader(new ProjectContext());
            var extensions = loader.Load("Plugins");
            var sampleSettingProvider = new SampleProjectSettingProvider();

            var editor = EditorFactory.CreateEditor(extensions, sampleSettingProvider);
            var vm = new EditorViewModel(editor, extensions);
            var main = new MainDataContext(vm, extensions);

            return main;
        }
    }
}
