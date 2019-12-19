using Romanesco.Common.Utility;
using Romanesco.View.DataContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Romanesco.View.View
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
            if (DataContext is ListContext listContext)
            {
                var button = (Button)sender;
                var index = listContext.Elements.IndexOf((StateViewContext)button.DataContext);
                listContext.ViewModel.RemoveCommand.Execute(index);
            }
        }
    }
}
