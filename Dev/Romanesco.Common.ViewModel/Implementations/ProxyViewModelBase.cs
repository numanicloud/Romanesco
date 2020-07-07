using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Reactive.Bindings;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.Common.ViewModel.Implementations
{
	public abstract class ProxyViewModelBase<TModel> : IStateViewModel
		where TModel : IFieldState
	{
		public virtual IReadOnlyReactiveProperty<string> Title => State.Title;
		public virtual IReadOnlyReactiveProperty<string> FormattedString => State.FormattedString;
		public virtual IObservable<Exception> OnError => State.OnError;
		public virtual IObservable<Unit> ShowDetail => ShowDetailSubject;
		public TModel State { get; }

		protected Subject<Unit> ShowDetailSubject { get; } = new Subject<Unit>();

		protected ProxyViewModelBase(TModel state)
		{
			State = state;
		}
	}
}
