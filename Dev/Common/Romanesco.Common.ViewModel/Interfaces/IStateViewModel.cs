using Reactive.Bindings;
using System;
using System.Reactive;

namespace Romanesco.Common.ViewModel.Interfaces
{
    /// <summary>
    /// 編集対象となるメンバーのビュー モデル。
    /// </summary>
    public interface IStateViewModel
    {
        /// <summary>
        /// 編集用のUIを説明するラベルのためのテキスト。
        /// </summary>
        /// <remarks>基本的にドメイン モデルにリダイレクトします。</remarks>
        IReadOnlyReactiveProperty<string> Title { get; }
        /// <summary>
        /// 入力値を説明する文字列形式。
        /// </summary>
        /// <remarks>
        /// クラス型のUIなどは、詳細を表示する操作が行われない限りは概略だけを表示します。
        /// その概略として表示される文字列です。
        /// 基本的にドメイン モデルにリダイレクトします。
        /// </remarks>
        IReadOnlyReactiveProperty<string> FormattedString { get; }
        /// <summary>
        /// このオブジェクトの詳細を表示する操作が行われた際に、このストリームに通知します。
        /// </summary>
        IObservable<Unit> ShowDetail { get; }
        /// <summary>
        /// メンバーの編集操作にあたってエラーが生じた場合、このストリームに通知します。
        /// </summary>
        IObservable<Exception> OnError { get; }
    }
}
