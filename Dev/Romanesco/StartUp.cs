using Romanesco.Common.Model.Basics;
using Romanesco.Extensibility;
using Romanesco.Sample;
using Romanesco.View;
using Romanesco.Model.EditorComponents;
using Romanesco.ViewModel;

namespace Romanesco
{
    class StartUp
    {
        public MainDataContext LoadMainDataContext()
        {
            var loader = new PluginLoader();
            var extensions = loader.Load("Plugins");
            var settingsProvider = new VmProjectSettingsProvider();

            var editor = EditorFactory.CreateEditor(extensions, settingsProvider);
            var vm = new EditorViewModel(editor, extensions);
            var main = new MainDataContext(vm, extensions);

            settingsProvider.ViewModel = vm;

            return main;
        }
    }
}
