using System;
using System.IO;

namespace Romanesco.Startup
{
	class RootExceptionHandler
	{
		public void ProcessError(Exception exception)
		{
#if DEBUG
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
