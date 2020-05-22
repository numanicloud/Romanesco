using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace Romanesco.BuiltinPlugin.Model.Infrastructure
{
    public class MasterList
    {
        public string MasterName { get; }
        public ListState State { get; }
        public string IdMemberName { get; }
        public Type IdType { get; }

        private readonly Func<object, object?> getter;

        public MasterList(string masterName, ListState state, string idMemberName)
        {
            MasterName = masterName;
            State = state;
            IdMemberName = idMemberName;

            if (State.ElementType.GetProperty(IdMemberName) is PropertyInfo prop)
            {
                IdType = prop.PropertyType;
                getter = obj =>
                {
                    var method = prop.GetGetMethod();
                    if (method == null)
                    {
                        throw new InvalidOperationException($"プロパティ{IdMemberName}のgetterを取得できませんでした。");
                    }
                    return method.Invoke(obj, null);
                };
            }
            else if (State.ElementType.GetField(IdMemberName) is FieldInfo field)
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
            var result = State.Elements.FirstOrDefault(state =>
                state.Storage.GetValue() is { } currentItem
                    && getter(currentItem) is { } currentId
                    && currentId.Equals(id));
            return result;
        }

        public object GetId(object subject)
        {
            return getter(subject) ?? throw new InvalidOperationException("指定されたオブジェクトのIDがnullです。");
        }
    }
}
