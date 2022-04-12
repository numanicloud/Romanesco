using System;

namespace Romanesco.Common.Model.Exceptions
{
    public class ContentAccessException : Exception
    {
        public ContentAccessException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public static ContentAccessException GetFormattedStringError(Exception exception)
        {
            return new ContentAccessException("インスタンスの文字列形式を取得できませんでした。文字列ビューは更新されません。", exception);
        }

        public static ContentAccessException GetSetterError(Exception exception)
        {
            return new ContentAccessException("インスタンス メンバーに値を代入できませんでした。", exception);
        }

        public static ContentAccessException GetListError(Exception exception)
        {
            return new ContentAccessException("リストの要素を操作できませんでした。", exception);
        }
    }
}
