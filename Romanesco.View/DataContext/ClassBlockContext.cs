using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Romanesco.View.DataContext
{
    public class ClassBlockContext
    {
        public object[] ChildViews { get; }

        public ClassBlockContext(object[] childViews)
        {
            ChildViews = childViews;
        }
    }
}
