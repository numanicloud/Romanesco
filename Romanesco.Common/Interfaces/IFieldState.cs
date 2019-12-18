using Reactive.Bindings;
using Romanesco.Common.Utility;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace Romanesco.Common
{
    /// <summary>
    /// 編集対象となるメンバーのドメイン モデル。
    /// </summary>
    public interface IFieldState
    {
        /// <summary>
        /// 編集用のUIを説明するラベルのためのテキスト。
        /// </summary>
        ReactiveProperty<string> Title { get; }
        /// <summary>
        /// 編集用のUIに表示し、実際に編集される入力値。
        /// </summary>
        ReactiveProperty<object> Content { get; }
        /// <summary>
        /// 入力値を説明する文字列形式。
        /// </summary>
        /// <remarks>
        /// クラス型のUIなどは、詳細を表示する操作が行われない限りは概略だけを表示します。
        /// その概略として表示される文字列です。
        /// </remarks>
        ReactiveProperty<string> FormattedString { get; }
        /// <summary>
        /// メンバーの型。ビュー モデルの生成などに使用されます。
        /// </summary>
        Type Type { get; }
        /// <summary>
        /// この要素の入力値が実際に代入される格納先を表します。
        /// シリアライズ/デシリアライズの際に使用されます。
        /// </summary>
        ValueSettability Settability { get; }
        /// <summary>
        /// メンバーの編集操作にあたってエラーが生じた場合、このストリームに通知します。
        /// </summary>
        IObservable<Exception> OnError { get; }
    }
}
