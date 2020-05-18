using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace Romanesco.BuiltinPlugin.Model.Factories
{
    public class ClassStateFactory : IStateFactory
    {
        public IFieldState? InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
        {
            EditorMemberAttribute? GetMemberAttributeOrDefault(MemberInfo member)
            {
                return member.GetCustomAttribute<EditorMemberAttribute>();
            }

            var type = settability.Type;
            if (!type.IsClass)
            {
                return null;
            }

            // 新規作成時は null である場合がある。ロード時は値が入ってるので上書き禁止
            if (settability.GetValue() == null)
            {
                if (Activator.CreateInstance(type) is object classInstance)
                {
                    settability.SetValue(classInstance);
                }
                else
                {
                    throw new InvalidOperationException($"{type.FullName}のインスタンスを生成できませんでした。");
                }
            }

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
