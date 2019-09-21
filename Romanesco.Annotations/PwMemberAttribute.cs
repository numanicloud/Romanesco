using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Annotations
{
    public class PwMemberAttribute : Attribute
    {
        public PwMemberAttribute(string title = null, int order = int.MaxValue)
        {
            Title = title;
            Order = order;
        }

        public string Title { get; }
        public int Order { get; }
    }
}
