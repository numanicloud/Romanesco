using System;
using System.Linq;
using System.Reactive.Linq;
using Romanesco.Common.View.Basics;

namespace Romanesco.View.Entry
{
    public class StateRootDataContext
    {
        public StateViewContext[] RootViews { get; }
        public IObservable<Exception> OnError { get; }

        public StateRootDataContext(StateViewContext[] roots)
        {
            RootViews = roots;
            OnError = Observable.Merge(roots.Select(x => x.OnError));
        }
    }
}
