using System;

namespace Romanesco.Common.Model.Helpers
{
	public class HandlingDisposable : IDisposable
    {
        private readonly Action onCompleted;

        public HandlingDisposable(Action onCompleted)
        {
            this.onCompleted = onCompleted;
        }

        public void Dispose()
        {
            onCompleted.Invoke();
        }
    }
}
