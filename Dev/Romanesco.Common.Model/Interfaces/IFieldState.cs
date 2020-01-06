using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using System;
using System.Reactive;

namespace Romanesco.Common.Model.Interfaces
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
        /// 入力値を説明する文字列形式。
        /// </summary>
        /// <remarks>
        /// クラス型のUIなどは、詳細を表示する操作が行われない限りは概略だけを表示します。
        /// その概略として表示される文字列です。
        /// </remarks>
        IReadOnlyReactiveProperty<string> FormattedString { get; }

        /// <summary>
        /// メンバーの型。ビュー モデルの生成などに使用されます。
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// この要素の入力値が実際に代入される格納先を表します。
        /// シリアライズ/デシリアライズの際に使用されます。
        /// </summary>
        ValueStorage Storage { get; }

        /// <summary>
        /// メンバーの編集操作にあたってエラーが生じた場合、このストリームに通知します。
        /// </summary>
        IObservable<Exception> OnError { get; }

        /// <summary>
        /// このメンバーの子孫メンバーのうちいずれかが編集されたときに、このストリームに通知します。
        /// </summary>
        /// <remarks>
        /// 再帰的な構造を持つ <see cref="IFieldState"/> を実装する場合、
        /// 子要素にあたる <see cref="IFieldState"/> の変更を全て回収し、
        /// 親要素に通知する必要があります。
        /// この手順により、全ての編集操作はモデルのルートに至るまで通知されます。
        /// </remarks>
        IObservable<Unit> OnEdited { get; }
    }
}
