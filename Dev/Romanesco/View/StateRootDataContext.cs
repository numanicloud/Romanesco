using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace Romanesco.View
{
    class StateRootDataContext
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
