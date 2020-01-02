using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Romanesco.Common.Utility
{
    /// <summary>
    /// プロパティ、フィールドというような、何らかの値を代入できるものを表します。
    /// </summary>
    public class ValueSettability
    {
        private Action<object, object, object[]> setter;

        public Type Type { get; }
        public string MemberName { get; }
        public Attribute[] Attributes { get; set; }

        /// <summary>
        /// プロパティをラップし、他の要素と同様に <see cref="ValueSettability"/> として扱えるようにします。
        /// </summary>
        /// <param name="property">プロパティを表す PropertyInfo。</param>
        public ValueSettability(PropertyInfo property)
        {
            setter = (subject, value, index) => property.SetValue(subject, value, index);
            Type = property.PropertyType;
            MemberName = property.Name;
            Attributes = property.GetCustomAttributes().ToArray();
        }

        /// <summary>
        /// フィールドをラップし、他の要素と同様に <see cref="ValueSettability"/> として扱えるようにします。
        /// </summary>
        /// <param name="field">フィールドを表す FieldInfo。</param>
        public ValueSettability(FieldInfo field)
        {
            setter = (subject, value, index) => field.SetValue(subject, value);
            Type = field.FieldType;
            MemberName = field.Name;
            Attributes = field.GetCustomAttributes().ToArray();
        }

        /// <summary>
        /// デリゲートをラップし、他の要素と同様に <see cref="ValueSettability"/> として扱えるようにします。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <param name="setter">
        ///     値の代入が要求されたときに呼び出されるデリゲート。
        ///     第1引数:メンバーを包含するオブジェクトへの参照。
        ///     第2引数:設定する値。
        ///     第3引数:インデクサーのインデックスなど、代入に関するその他の情報の配列。
        /// </param>
        public ValueSettability(Type type, string memberName, Action<object, object, object[]> setter)
        {
            this.setter = setter;
            Type = type;
            MemberName = memberName;
            Attributes = new Attribute[0];
        }

        /// <summary>
        /// この要素が表す格納先に値を代入します。
        /// </summary>
        /// <param name="subject">メンバーを包含するオブジェクトへの参照。</param>
        /// <param name="value">設定する値。</param>
        /// <param name="context">インデクサーのインデックスなど、代入に関するその他の情報の配列。</param>
        public void SetValue(object subject, object value, object[] context = null)
        {
            setter.Invoke(subject, value, context);
        }
    }
}
