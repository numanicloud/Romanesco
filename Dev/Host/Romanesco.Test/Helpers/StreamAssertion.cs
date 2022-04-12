using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Romanesco.Test.Helpers
{
	/// <summary>
	/// Disposeされるまでに、与えたストリームに値が流れてこなければアサーションを失敗させます。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	class StreamAssertion<T> : IDisposable
	{
		private bool isRaised = false;
		private readonly IDisposable subscription;

		public StreamAssertion(IObservable<T> stream)
		{
			subscription = stream.Subscribe(x => isRaised = true);
		}

		public void Dispose()
		{
			subscription.Dispose();
			Assert.True(isRaised);
		}
	}

	static class StreamAssertionExtension
	{
		public static StreamAssertion<T> ExpectAtLeastOnce<T>(this IObservable<T> stream)
		{
			return new StreamAssertion<T>(stream);
		}
	}
}
