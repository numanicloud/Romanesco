using Romanesco.Extensibility;
using Romanesco.Model;
using Romanesco.Sample;
using Romanesco.View;
using System.Reactive.Linq;
using System;
using System.Threading.Tasks;
using Romanesco.Common.Model.Basics;

namespace Romanesco
{
    class StartUp
    {
        private readonly MementoSola.Altseed.Debug.RootExceptionHandler logger;

        public MainDataContext LoadMainDataContext()
        {
            var loader = new PluginLoader(new ProjectContext());
            var extensions = loader.Load("Plugins");
            var sampleSettingProvider = new SampleProjectSettingProvider();

            var editor = new Editor(extensions, sampleSettingProvider);
            var vm = new EditorViewModel(editor, extensions);
            var main = new MainDataContext(vm, extensions);

            return main;
        }
    }
}
