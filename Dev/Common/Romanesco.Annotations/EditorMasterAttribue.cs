
using System;

namespace Romanesco.Annotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class EditorMasterAttribute : Attribute
    {
        public EditorMasterAttribute(string masterName, string idMemberName)
        {
            MasterName = masterName;
            IdMemberName = idMemberName;
        }

        public string MasterName { get; }
        public string IdMemberName { get; }
    }
}