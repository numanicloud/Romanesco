using Romanesco.BuiltinPlugin.View.DataContext;
using Romanesco.Common.View.Basics;
using System.Windows;
using System.Windows.Controls;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.View.View2
{
	/// <summary>
	/// PrimitiveListBlockView.xaml の相互作用ロジック
	/// </summary>
	public partial class PrimitiveListBlockView : UserControl
    {
        public PrimitiveListBlockView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is PrimitiveListViewModel listContext)
            {
                var button = (Button)sender;
                var index = listContext.Elements.IndexOf((IStateViewModel)button.DataContext);
                listContext.RemoveCommand.Execute(index);
            }
        }
    }
}
