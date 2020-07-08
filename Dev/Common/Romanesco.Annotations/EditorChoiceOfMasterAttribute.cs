
using System;

namespace Romanesco.Annotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class EditorChoiceOfMasterAttribute : Attribute
    {
        public EditorChoiceOfMasterAttribute(string masterName)
        {
            MasterName = masterName;
        }

        public string MasterName { get; }
    }
}