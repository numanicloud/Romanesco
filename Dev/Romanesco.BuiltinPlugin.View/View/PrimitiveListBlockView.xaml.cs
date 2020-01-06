using Romanesco.BuiltinPlugin.View.DataContext;
using Romanesco.Common.View.Basics;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Romanesco.BuiltinPlugin.View.View
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
