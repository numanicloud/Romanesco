using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Reactive.Bindings.Extensions;
using static System.Collections.Specialized.NotifyCollectionChangedAction;

namespace Romanesco.Common.Model.Helpers
{
    public static class Helper
    {
        public static event Action<Exception>? OnUnhandledExceptionRaisedInSubscribe;

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
        /// <see cref="OnUnhandledExceptionRaisedInSubscribe"/> に例外が通知されます。
        /// </summary>
        /// <typeparam name="T">イベントの型。</typeparam>
        /// <param name="source">ストリーム。</param>
        /// <param name="onNext">イベントをハンドリングするデリゲート。</param>
        /// <returns></returns>
        public static IDisposable SubscribeSafe<T>(this IObservable<T> source,
            Action<T> onNext, Action<Exception>? onError = null, Action? onCompleted = null)
        {
            return source.Subscribe(x =>
            {
                try
                {
                    onNext(x);
                }
                catch (Exception ex)
                {
                    OnUnhandledExceptionRaisedInSubscribe?.Invoke(ex);
                }
            }, exception =>
            {
                try
                {
                    onError?.Invoke(exception);
                }
                catch (Exception ex)
                {
                    OnUnhandledExceptionRaisedInSubscribe?.Invoke(ex);
                }
            }, () =>
            {
                try
                {
                    onCompleted?.Invoke();
                }
                catch (Exception ex)
                {
                    OnUnhandledExceptionRaisedInSubscribe?.Invoke(ex);
                }
            });
        }

        public static (ObservableCollection<U> result, IDisposable disposable) ToObservableCollection<T, U>(
            this ObservableCollection<T> source,
            Func<T, U> converter)
        {
            var proxy = new ObservableCollection<U>();
            var disposable = new CompositeDisposable();

            source.CollectionChangedAsObservable().Where(x => x.Action == Add)
                .Select(x => new { Index = x.NewStartingIndex, Item = x.NewItems!.Cast<T>().First() })
                .Subscribe(x => proxy.Insert(x.Index, converter(x.Item)))
                .AddTo(disposable);

            source.CollectionChangedAsObservable().Where(x => x.Action == Move)
                .Subscribe(x => proxy.Move(x.OldStartingIndex, x.NewStartingIndex))
                .AddTo(disposable);

            source.CollectionChangedAsObservable().Where(x => x.Action == Remove)
                .Select(x => new { OldIndex = x.OldStartingIndex })
                .Subscribe(x => proxy.RemoveAt(x.OldIndex))
                .AddTo(disposable);

            source.CollectionChangedAsObservable().Where(x => x.Action == Replace)
                .Select(x => new { Index = x.NewStartingIndex, Item = x.NewItems!.Cast<T>().First() })
                .Subscribe(x => proxy[x.Index] = converter(x.Item))
                .AddTo(disposable);

            return (proxy, disposable);
        }

        public static void Forget(this Task task)
        {
            var ignore = task;
        }

        public static IEnumerable<T> StartsWith<T>(this IEnumerable<T> source, T value)
        {
            yield return value;
            foreach (var item in source)
            {
                yield return item;
            }
        }
    }
}
