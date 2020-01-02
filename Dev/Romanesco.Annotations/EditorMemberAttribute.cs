﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Annotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class EditorMemberAttribute : Attribute
    {
        public EditorMemberAttribute(string title = null, int order = int.MaxValue)
        {
            Title = title;
            Order = order;
        }

        public string Title { get; }
        public int Order { get; }
    }
}