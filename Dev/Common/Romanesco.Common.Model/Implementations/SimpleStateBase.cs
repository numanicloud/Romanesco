using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.Model.Interfaces;

namespace Romanesco.Common.Model.Implementations
{
	public abstract class SimpleStateBase : IFieldState
	{
		public virtual void Dispose()
		{
			Title.Dispose();
			FormattedString.Dispose();
			OnErrorSubject.Dispose();
			foreach (var disposable in Disposables)
			{
				disposable.Dispose();
			}
		}

		public ReactiveProperty<string> Title { get; }
		public abstract IReadOnlyReactiveProperty<string> FormattedString { get; }
		public virtual Type Type => Storage.Type;
		public virtual ValueStorage Storage { get; }
		public virtual IObservable<Exception> OnError => OnErrorSubject;
		public IObservable<Unit> OnEdited { get; protected set; }
		public List<IDisposable> Disposables { get; } = new List<IDisposable>();

		protected Subject<Exception> OnErrorSubject { get; } = new Subject<Exception>();

		protected SimpleStateBase(ValueStorage storage)
		{
			Storage = storage;
			Title = new ReactiveProperty<string>(storage.MemberName);
			OnEdited = storage.OnValueChanged.ToUnit();
		}

		protected string SanitizeNewFormattedString(object? newValue)
		{
			try
			{
				return newValue?.ToString() ?? "<null>";
			}
			catch (Exception e)
			{
				OnErrorSubject.OnNext(ContentAccessException.GetFormattedStringError(e));
				return "<error>";
			}
		}
	}
}
