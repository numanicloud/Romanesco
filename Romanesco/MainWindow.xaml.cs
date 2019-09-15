using Romanesco.Extensibility;
using Romanesco.Common;
using Romanesco.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Romanesco.Annotations;

namespace Romanesco {
    public class Hoge
    {
        [PwMember]
        public int Integer { get; set; }
        [PwMember]
        public bool Boolean { get; set; }
        [PwMember]
        public string String { get; set; }
        [PwMember]
        public float Float { get; set; }
        [PwMember]
        public byte Byte;
        [PwMember]
        public short Short;
        [PwMember]
        public long Long;
        [PwMember]
        public double Double;
        public int Hidden;
    }

    public class Project
    {
        public Hoge Hoge { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            var loader = new PluginLoader();
            var extensions = loader.Load("Plugins");
            var stateInterpreter = new ObjectInterpreter(extensions.StateFactories);
            var viewModelInterpreter = new ViewModel.ViewModelInterpreter(extensions.StateViewModelFactories);
            var viewInterpreter = new View.ViewInterpreter(extensions.ViewFactories);

            var hogeProperty = typeof(Project).GetProperty("Hoge");
            var state = stateInterpreter.InterpretAsState(hogeProperty);
            var viewModel = viewModelInterpreter.InterpretAsViewModel(state);
            var view = viewInterpreter.InterpretAsView(viewModel);

            DataContext = new { Root = view };
        }
    }
}
