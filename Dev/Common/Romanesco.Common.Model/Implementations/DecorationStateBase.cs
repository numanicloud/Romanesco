using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace Romanesco.Common.Model.Implementations
{
	public abstract class DecorationStateBase : IFieldState
	{
		public virtual ReactiveProperty<string> Title => BaseField.Value.Title;
		public virtual IReadOnlyReactiveProperty<string> FormattedString => BaseField.Value.FormattedString;
		public virtual Type Type => BaseField.Value.Type;
		public virtual ValueStorage Storage => BaseField.Value.Storage;
		public virtual IObservable<Exception> OnError => BaseField.Value.OnError;
		public virtual IObservable<Unit> OnEdited => BaseField.Value.OnEdited;

		protected ReactiveProperty<IFieldState> BaseField { get; }
		public List<IDisposable> Disposables { get; } = new List<IDisposable>();

		protected DecorationStateBase(IFieldState initialBaseState)
		{
			BaseField = new ReactiveProperty<IFieldState>(initialBaseState, ReactivePropertyMode.DistinctUntilChanged);
		}

		public virtual void Dispose()
		{
			BaseField.Value.Dispose();
			BaseField.Dispose();
			Disposables.ForEach(d => d.Dispose());
		}
	}
}
