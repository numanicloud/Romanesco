
using System;

namespace Romanesco.Annotations
{
    public class EditorChoiceMemberAttribute : Attribute
    {
        public EditorChoiceMemberAttribute(string masterName, string title = null, int order = int.MaxValue)
        {
            MasterName = masterName;
            Title = title;
            Order = order;
        }

        public string MasterName { get; }
        public string Title { get; }
        public int Order { get; }
    }
}