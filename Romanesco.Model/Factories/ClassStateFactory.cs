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
        public IFieldState InterpretAsState(ValueSettability settability, StateInterpretFunc interpret)
        {
            EditorMemberAttribute GetMemberAttributeOrDefault(MemberInfo member)
            {
                return member.GetCustomAttribute(typeof(EditorMemberAttribute)) as EditorMemberAttribute;
            }

            var type = settability.Type;
            if (!type.IsClass)
            {
                return null;
            }

            var properties = from p in type.GetProperties()
                             let attr = GetMemberAttributeOrDefault(p)
                             where attr != null
                             select (state: interpret(new ValueSettability(p)), attr);
            var fields = from f in type.GetFields()
                         let attr = GetMemberAttributeOrDefault(f)
                         where attr != null
                         select (state: interpret(new ValueSettability(f)), attr);
            var members = properties.Concat(fields).OrderBy(x => x.attr.Order);

            foreach (var m in members)
            {
                if (m.attr.Title != null)
                {
                    m.state.Title.Value = m.attr.Title;
                }
            }

            var memberStates = members.Select(x => x.state).ToArray();
            return new ClassState(settability, memberStates);
        }
    }
}
