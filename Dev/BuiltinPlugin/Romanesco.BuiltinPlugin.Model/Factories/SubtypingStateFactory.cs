using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace Romanesco.BuiltinPlugin.Model.Factories
{
	public class SubtypingStateFactory : IStateFactory
	{
		private readonly SubtypingContext context = new SubtypingContext();
		private readonly IServiceLocator serviceLocator;

		public SubtypingStateFactory(IServiceLocator serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

		public IFieldState? InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
		{
			Type type = settability.Type;
			if (!type.IsClass
				|| type.GetCustomAttribute<EditorSubtypingBaseAttribute>() is null)
			{
				return null;
			}

			SubtypingList? list = context.Get(type);
			if (list is null)
			{
				list = new SubtypingList(type);
				context.Add(type, list);

				// TODO: 指定された型と別のアセンブリの内容も読み込みたい
				type.Assembly.GetExportedTypes()
					.Where(x => type.IsAssignableFrom(x))
					.Where(x => type != x)
					.ForEach(x => list.Add(x));
			}


			return new SubtypingClassState(settability, list, serviceLocator);
		}
	}
}
