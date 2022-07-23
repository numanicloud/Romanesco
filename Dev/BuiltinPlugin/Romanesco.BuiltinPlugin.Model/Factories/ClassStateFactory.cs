using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NacHelpers.Extensions;
using Romanesco.Common.Model.Reflections;

namespace Romanesco.BuiltinPlugin.Model.Factories
{
    public class ClassStateFactory : IStateFactory
    {
		private readonly IDataAssemblyRepository asmRepo;
		private readonly ILoadingStateReader _loadingState;

		public ClassStateFactory(IDataAssemblyRepository asmRepo, ILoadingStateReader loadingState)
		{
			this.asmRepo = asmRepo;
			_loadingState = loadingState;
		}

        public IFieldState? InterpretAsState(ValueStorage storage, StateInterpretFunc interpret)
		{
			var type = storage.Type;
			if (!type.IsClass)
			{
				return null;
			}
			
			_loadingState.BreakIfNotLoading();

			// 新規作成時は null である場合がある。逆にロード時は値が入ってるので上書き禁止
			object subject = GetOrCreateInstance(storage);

			ClassState.Property[] MakeClassMembers()
			{
				// インデクサーは取り除く
				var properties = from p in type.GetProperties()
					where p.GetIndexParameters().IsEmpty()
					let order = GetMemberAttributesOrder(p)
					select (state: interpret(new ValueStorage(subject, p)), order, name: p.Name);

				var fields = from f in type.GetFields()
					let order = GetMemberAttributesOrder(f)
					select (state: interpret(new ValueStorage(subject, f)), order, name: f.Name);

				var members = from m in properties.Concat(fields)
					orderby m.order switch
					{
						Ordered ordered => ordered.Order,
						Nothing _ => -1,
						_ => throw new Exception()
					}
					where !(m.order is Nothing)
					select new ClassState.Property(m.name, m.state);

				return members.FilterNullRef().ToArray();
			}

			return new ClassState(storage, MakeClassMembers());
		}

		private object GetOrCreateInstance(ValueStorage storage)
		{
			if (storage.GetValue() is { } subject)
			{
			}
			else
			{
				var type = storage.Type;
				if (type == typeof(string))
				{
					var str = "";
					storage.SetValue(str);
					subject = str;
				}
				else if (asmRepo.CreateInstance(type) is { } classInstance)
				{
					storage.SetValue(classInstance);
					subject = classInstance;
				}
				else
				{
					throw new InvalidOperationException($"{type.FullName}のインスタンスを生成できませんでした。");
				}
			}

			return subject;
		}
		
		private static IEditorMemberValue GetMemberAttributesOrder(MemberInfo member)
		{
			var attribute = member.GetCustomAttributesData()
				.FirstOrDefault(x =>
				{
					return x.AttributeType.FullName == typeof(EditorMemberAttribute).FullName;
				});

			if (attribute is null)
			{
				return new Nothing();
			}

			var argument = attribute.ConstructorArguments
				.FirstOrDefault(x => x.ArgumentType == typeof(int));

			if (argument.Value is null)
			{
				return new Ordered(default);
			}

			var orderValue = (int) argument.Value;
			return new Ordered(orderValue);
		}
		
		private interface IEditorMemberValue
		{
		}
		private struct Ordered : IEditorMemberValue
		{
			public Ordered(int order)
			{
				Order = order;
			}

			public int Order { get; }
		}

		private struct Nothing : IEditorMemberValue
		{	
		}
	}
}
