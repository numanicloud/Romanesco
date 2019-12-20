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
        [EditorMember]
        public float X { get; set; }
        [EditorMember]
        public float Y { get; set; }
        [EditorMember]
        public float Z { get; set; }
        [EditorMember]
        public List<int> IntList { get; set; } = new List<int>();

        public override string? ToString()
        {
            return $"Fuga:({X}, {Y}, {Z}, List={IntList.Count})";
        }
    }

    public class Hoge
    {
        [EditorMember(order: 0)]
        public int Integer { get; set; }
        [EditorMember(order: 1)]
        public bool Boolean { get; set; }
        [EditorMember(order: 2)]
        public string String { get; set; }
        [EditorMember(order: 3)]
        public float Float { get; set; }
        [EditorMember(order: 4)]
        public byte Byte;
        [EditorMember(order: 5)]
        public short Short;
        [EditorMember(order: 6)]
        public long Long;
        [EditorMember(order: 7)]
        public double Double;
        public int Hidden;
        [EditorMember(order: 8)]
        public Fuga Fuga { get; set; }
        [EditorMember(order: 9)]
        public List<Fuga> FugaList { get; set; } = new List<Fuga>();

        public override string? ToString()
        {
            return $"Fuga x{FugaList.Count}";
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
            view.OnError.Subscribe(ex =>
            {
                throw ex;
            });

            DataContext = new { Root = view };
        }
    }
}
