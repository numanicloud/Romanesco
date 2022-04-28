using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Romanesco.BuiltinPlugin.Model.Factories
{
	public class SubtypingStateFactory : IStateFactory
	{
		private readonly IApiFactory api;
		private readonly ClassStateFactory _factory;
		private readonly SubtypingContext context = new SubtypingContext();

		public SubtypingStateFactory(IApiFactory api, ClassStateFactory factory)
		{
			this.api = api;
			_factory = factory;
		}

		public IFieldState? InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
		{
			Debug.WriteLine("Trying interpret as SubtypingClassState.");

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


			var stateContext = new SubtypingStateContext(list, api.ResolveDataAssemblyRepository(), api.ResolveObjectInterpreter());
			return new SubtypingClassState(settability, stateContext, _factory);
		}
	}
}
