
using System;

namespace Romanesco.Annotations
{
    public class EditorMasterAttribute : Attribute
    {
        public EditorMasterAttribute(string masterName, string idMemberName, string title = null, int order = int.MaxValue)
        {
            MasterName = masterName;
            IdMemberName = idMemberName;
            Title = title;
            Order = order;
        }

        public string MasterName { get; }
        public string IdMemberName { get; }
        public string Title { get; }
        public int Order { get; }
    }
}