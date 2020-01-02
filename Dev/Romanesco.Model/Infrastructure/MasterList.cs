using Romanesco.Common;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Romanesco.Model.Infrastructure
{
    public class MasterList
    {
        public string MasterName { get; }
        public ListState State { get; }
        public string IdMemberName { get; }
        public Type IdType { get; }

        private readonly Func<object, object> getter;

        public MasterList(string masterName, ListState state, string idMemberName)
        {
            MasterName = masterName;
            State = state;
            IdMemberName = idMemberName;

            if (State.ElementType.GetProperty(IdMemberName) is PropertyInfo prop)
            {
                IdType = prop.PropertyType;
                getter = obj => prop.GetGetMethod().Invoke(obj, null);
            }
            else if(State.ElementType.GetField(IdMemberName) is FieldInfo field)
            {
                IdType = field.FieldType;
                getter = obj => field.GetValue(obj);
            }
            else
            {
                throw new Exception("Given master is not a list of class/struct.");
            }
        }

        public IFieldState GetById(object id)
        {
            return State.Elements.FirstOrDefault(state => getter(state.Storage.GetValue()) == id);
        }

        public object GetId(object subject)
        {
            return getter(subject);
        }
    }
}
