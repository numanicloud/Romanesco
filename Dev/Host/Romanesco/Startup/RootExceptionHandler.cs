using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NacHelpers.Extensions;

namespace Romanesco.Startup
{
	internal class RootExceptionHandler
	{
		public void ProcessError(Exception exception)
		{
#if DEBUG
			var exceptionList = CrawlExceptions(exception, 0)
				.ToLinear()
				.OrderBy(x => x.Item2)
				.Select(x => x.Item1)
				.ToArray();
			throw exception;
#else
			if (exception is AggregateException aggregated)
			{
				ProcessError(aggregated, (stream, e) =>
				{
					WriteException(stream, aggregated);
					foreach (var inner in aggregated.InnerExceptions)
					{
						stream.WriteLine();
						WriteException(stream, inner);
					}
				});
			}
			else
			{
				ProcessError(exception, (stream, e) => WriteException(stream, e));
			}
#endif
		}

		private static IEnumerable<IEnumerable<(Exception, int)>> CrawlExceptions(Exception exception, int depth)
		{
			IEnumerable<(Exception, int)> Recurse(Exception e) => CrawlExceptions(e, depth + 1).ToLinear();

			yield return new []{ (exception, depth) };

			if (exception is AggregateException aException)
			{
				foreach (var innerException in aException.InnerExceptions)
				{
					if (innerException is {})
					{
						yield return Recurse(innerException);
					}
				}
			}
			else if (exception.InnerException is { } inner)
			{
				yield return Recurse(inner);
			}

			if (exception is ReflectionTypeLoadException rtlException
			    && rtlException.LoaderExceptions is { })
			{
				foreach (var loaderException in rtlException.LoaderExceptions)
				{
					if (loaderException is { })
					{
						yield return Recurse(loaderException);
					}
				}
			}
		}

		private void ProcessError<TException>(TException ex, Action<StreamWriter, TException> write)
		{
			var footer = DateTime.Now.ToString("yyyyMMdd_HHmm");
			var fileName = $"Romanesco-error{footer}.txt";
			using (var log = new StreamWriter(fileName))
			{
				write(log, ex);
				log.WriteLine();
			}
			System.Windows.MessageBox.Show($"エラーが発生しました。{fileName}にログを書き込みました。");
		}

		private static void WriteException(StreamWriter log, Exception ex)
		{
			log.WriteLine(ex.GetType().Name);
			log.WriteLine(ex.Message);
			log.WriteLine(ex.StackTrace);
		}
	}
}
