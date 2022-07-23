using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using Romanesco.Common.View.Basics;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.View.Entry
{
    public class StateRootDataContext
    {
        public StateViewContext[] RootViews { get; }
        public IStateViewModel[] ViewModels { get; }
		public IObservable<Exception> OnError { get; }
        public StateRoot[] UserControls { get; }

        public StateRootDataContext
			(StateViewContext[] roots,
			IStateViewModel[] viewModels, UserControl[] userControls)
        {
            RootViews = roots;
			ViewModels = viewModels;
			UserControls = userControls.Select(x => new StateRoot(x)).ToArray();
			OnError = Observable.Merge(roots.Select(x => x.OnError));
		}
    }

	public class StateRoot
	{
		public UserControl Control { get; }

		public StateRoot(UserControl control)
		{
			Control = control;
		}
	}
}
