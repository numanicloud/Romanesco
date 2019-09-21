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
    public class Fuga
    {
        [PwMember]
        public float X { get; set; }
        [PwMember]
        public float Y { get; set; }
        [PwMember]
        public float Z { get; set; }

        public override string? ToString()
        {
            return $"Fuga:({X}, {Y}, {Z})";
        }
    }

    public class Hoge
    {
        [PwMember(order: 0)]
        public int Integer { get; set; }
        [PwMember(order: 1)]
        public bool Boolean { get; set; }
        [PwMember(order: 2)]
        public string String { get; set; }
        [PwMember(order: 3)]
        public float Float { get; set; }
        [PwMember(order: 4)]
        public byte Byte;
        [PwMember(order: 5)]
        public short Short;
        [PwMember(order: 6)]
        public long Long;
        [PwMember(order: 7)]
        public double Double;
        public int Hidden;
        [PwMember(order: 8)]
        public Fuga Fuga { get; set; }
        [PwMember(order: 9)]
        public List<int> IntArray { get; set; }
        [PwMember(order: 10)]
        public List<Fuga> FugaArray { get; set; }

        public override string? ToString()
        {
            return $"Integer = {Integer}";
        }
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
