using Reactive.Bindings;
using System;

namespace Romanesco.Common.Model.Helpers
{
	public class BooleanUsingScopeSource
    {
        public ReactiveProperty<bool> IsUsing { get; } = new ReactiveProperty<bool>();

        public HandlingDisposable Create()
        {
            if (IsUsing.Value)
            {
                throw new InvalidOperationException("This scope is locked.");
            }

            IsUsing.Value = true;
            return new HandlingDisposable(() => IsUsing.Value = false);
        }
    }
}
