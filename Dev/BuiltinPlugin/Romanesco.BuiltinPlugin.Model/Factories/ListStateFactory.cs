using System;
using System.Collections;
using Romanesco.Annotations;
using Romanesco.BuiltinPlugin.Model.Infrastructure;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Romanesco.BuiltinPlugin.Model.Factories
{
    public class ListStateFactory : IStateFactory
    {
        private readonly MasterListContext context;
        private readonly CommandHistory history;

        public ListStateFactory(MasterListContext context, CommandHistory history)
        {
            this.context = context;
            this.history = history;
        }

        public IFieldState? InterpretAsState(ValueStorage settability, StateInterpretFunc interpret)
        {
            if (settability.Type.IsGenericType
                && settability.Type.GetGenericTypeDefinition() == typeof(List<>)
                && TryLoadOrCreateList(settability) is { } listInstance)
            {
                var list = new ListState(settability, listInstance, interpret, history);
                var attr = settability.Attributes.OfType<EditorMasterAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    context.AddList(new MasterList(attr.MasterName, list, attr.IdMemberName));
                }
                return list;
            }
            return null;
        }

        private IList? TryLoadOrCreateList(ValueStorage storage)
        {
	        if (storage.GetValue() is IList list)
	        {
		        return list;
	        }

	        var elementType = storage.Type.GetGenericArguments()[0];
	        if (Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType)) is IList newList)
	        {
                storage.SetValue(newList);
		        return newList;
	        }

	        return null;
        }
    }
}
