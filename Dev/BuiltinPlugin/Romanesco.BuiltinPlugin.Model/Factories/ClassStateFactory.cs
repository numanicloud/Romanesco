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
		private readonly IDataAssemblyRepository asmRepo;

		public ClassStateFactory(IDataAssemblyRepository asmRepo)
		{
			this.asmRepo = asmRepo;
		}

        public IFieldState? InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
		{
			var type = settability.Type;
			if (!type.IsClass)
			{
				return null;
			}

			// 新規作成時は null である場合がある。逆にロード時は値が入ってるので上書き禁止
			object subject = GetOrCreateInstance(settability);

			IFieldState[] memberStates = subject is DynamicMock mock
				? MakeDynamicMockMembers(type, mock)
				: MakeClassMembers(type, subject);

			return new ClassState(settability, memberStates);

			EditorMemberAttribute? GetMemberAttributeOrDefault(MemberInfo member)
			{
				return member.GetCustomAttribute<EditorMemberAttribute>();
			}

			IFieldState[] MakeClassMembers(Type type, object subject)
			{
				var properties = from p in type.GetProperties()
								 let attr = GetMemberAttributeOrDefault(p)
								 where attr != null
								 select (state: interpret(new ValueStorage(subject, p)), attr);
				var fields = from f in type.GetFields()
							 let attr = GetMemberAttributeOrDefault(f)
							 where attr != null
							 select (state: interpret(new ValueStorage(subject, f)), attr);
				var members = properties.Concat(fields).OrderBy(x => x.attr.Order).ToArray();

				var memberStates = members.Select(x => x.state).ToArray();
				return memberStates;
			}

			IFieldState[] MakeDynamicMockMembers(Type type, DynamicMock subject)
			{
				var factory = new ValueStorageFactory();
				return subject.Keys
					.Select(x => interpret(factory.FromDynamicMocksMember(subject, x)))
					.Where(x => x != null)
					.ToArray()!;
			}
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
