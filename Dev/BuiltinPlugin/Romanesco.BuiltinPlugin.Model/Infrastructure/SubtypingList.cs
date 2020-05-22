using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;

namespace Romanesco.BuiltinPlugin.Model.Infrastructure
{
	public class SubtypingList
	{
		private readonly List<Type> derivedTypes = new List<Type>();
		private readonly Subject<Type> newEntrySubject = new Subject<Type>();

		public Type BaseType { get; }
		public IList<Type> DerivedTypes => derivedTypes;
		public IObservable<Type> OnNewEntry => newEntrySubject;

		public SubtypingList(Type baseType)
		{
			BaseType = baseType;
		}

		public void Add(Type derivedType)
		{
			derivedTypes.Add(derivedType);
			newEntrySubject.OnNext(derivedType);
		}
	}
}
