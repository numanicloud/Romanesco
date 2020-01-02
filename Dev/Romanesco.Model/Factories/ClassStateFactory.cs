using Romanesco.Annotations;
using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Romanesco.Model.Factories
{
    public class ClassStateFactory : IStateFactory
    {
        public IFieldState InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
        {
            EditorMemberAttribute GetMemberAttributeOrDefault(MemberInfo member)
            {
                return member.GetCustomAttribute<EditorMemberAttribute>();
            }

            var type = settability.Type;
            if (!type.IsClass)
            {
                return null;
            }

            settability.SetValue(Activator.CreateInstance(type));

            var properties = from p in type.GetProperties()
                             let attr = GetMemberAttributeOrDefault(p)
                             where attr != null
                             select (state: interpret(new ValueStorage(settability.GetValue(), p)), attr);
            var fields = from f in type.GetFields()
                         let attr = GetMemberAttributeOrDefault(f)
                         where attr != null
                         select (state: interpret(new ValueStorage(settability.GetValue(), f)), attr);
            var members = properties.Concat(fields).OrderBy(x => x.attr.Order).ToArray();

            var memberStates = members.Select(x => x.state).ToArray();
            return new ClassState(settability, memberStates);
        }
    }
}
