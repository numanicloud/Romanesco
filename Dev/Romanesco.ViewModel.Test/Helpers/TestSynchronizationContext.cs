using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Romanesco.ViewModel.Test.Helpers
{
	class TestSynchronizationContext : SynchronizationContext
	{
		public override void Post(SendOrPostCallback d, object? state)
		{
			d(state);
		}

		public override void Send(SendOrPostCallback d, object? state)
		{
			d(state);
		}
	}
}
