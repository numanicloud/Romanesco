using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.Common.Utility
{
    public static class Helper
    {
        public static event Action<Exception> OnUnhandledExceptionRaisedInSubscribe;

        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> condition)
        {
            int index = 0;
            foreach (var item in source)
            {
                if (condition(item))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        /// <summary>
        /// ストリームの通知を購読します。引数に渡したデリゲート内部でエラーが発生した場合、
        /// <see cref="Helper.OnUnhandledExceptionRaisedInSubscribe"/> に例外が通知されます。
        /// </summary>
        /// <typeparam name="T">イベントの型。</typeparam>
        /// <param name="source">ストリーム。</param>
        /// <param name="onNext">イベントをハンドリングするデリゲート。</param>
        /// <returns></returns>
        public static IDisposable SubscribeSafe<T>(this IObservable<T> source,
            Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
        {
            return source.Subscribe(x =>
            {
                try
                {
                    onNext(x);
                }
                catch (Exception ex)
                {
                    OnUnhandledExceptionRaisedInSubscribe(ex);
                }
            }, exception =>
            {
                try
                {
                    onError?.Invoke(exception);
                }
                catch (Exception ex)
                {
                    OnUnhandledExceptionRaisedInSubscribe(ex);
                }
            }, () =>
            {
                try
                {
                    onCompleted?.Invoke();
                }
                catch (Exception ex)
                {
                    OnUnhandledExceptionRaisedInSubscribe(ex);
                }
            });
        }
    }
}
