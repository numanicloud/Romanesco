using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Annotations
{
    public class PwMemberAttribute : Attribute
    {
        public PwMemberAttribute(string title = null)
        {
            Title = title;
        }

        public string Title { get; }
    }
}
