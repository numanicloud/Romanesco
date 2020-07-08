using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Linq;
using System.Reflection;
using NacHelpers.Extensions;
using Romanesco.Common.Model.Reflections;

namespace Romanesco.BuiltinPlugin.Model.Factories
{
    public class ClassStateFactory : IStateFactory
    {
		private readonly IDataAssemblyRepository asmRepo;

		public ClassStateFactory(IDataAssemblyRepository asmRepo)
		{
			this.asmRepo = asmRepo;
		}

        public IFieldState? InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
		{
			EditorMemberAttribute? GetMemberAttributeOrDefault(MemberInfo member)
			{
				return member.GetCustomAttribute<EditorMemberAttribute>();
			}
			IFieldState[] MakeClassMembers(Type t, object o)
			{
				var properties = from p in t.GetProperties()
					let attr = GetMemberAttributeOrDefault(p)
					select (state: interpret(new ValueStorage(o, p)), attr);
				var fields = from f in t.GetFields()
					let attr = GetMemberAttributeOrDefault(f)
					select (state: interpret(new ValueStorage(o, f)), attr);
				return properties.Concat(fields)
					.Where(x => x.attr != null)
					.OrderBy(x => x.attr!.Order)
					.Select(x => x.state)
					.FilterNullRef()
					.ToArray();
			}

			var type = settability.Type;
			if (!type.IsClass)
			{
				return null;
			}

			// 新規作成時は null である場合がある。逆にロード時は値が入ってるので上書き禁止
			object subject = GetOrCreateInstance(settability);
			IFieldState[] memberStates = MakeClassMembers(type, subject);
			return new ClassState(settability, memberStates);
		}

		private object GetOrCreateInstance(ValueStorage settability)
		{
			var type = settability.Type;
			if (settability.GetValue() is { } subject)
			{
			}
			else
			{
				if (asmRepo.CreateInstance(type) is object classInstance)
				{
					settability.SetValue(classInstance);
					subject = classInstance;
				}
				else
				{
					throw new InvalidOperationException($"{type.FullName}のインスタンスを生成できませんでした。");
				}
			}

			return subject;
		}
	}
}
