using Romanesco.Extensibility;
using Romanesco.Interface;
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

namespace Romanesco {
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

            var state = stateInterpreter.InterpretAsState(20, "Root");
            var viewModel = viewModelInterpreter.InterpretAsViewModel(state);
            var view = viewInterpreter.InterpretAsView(viewModel);

            DataContext = new { Root = view };
        }
    }
}
