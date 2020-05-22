using Romanesco.Annotations;
using System;
using System.Linq;
using System.Reactive.Subjects;
using System.Reflection;

namespace Romanesco.Common.Model.Basics
{
    public delegate void SetterFunction(object? value, object? oldValue);

    /// <summary>
    /// プロパティ、フィールドというような、何らかの値を代入できるものを表します。
    /// </summary>
    public class ValueStorage
    {
        private readonly SetterFunction setter;
        private readonly Subject<object?> onValueChangedSubject = new Subject<object?>();
        private readonly Subject<(object? value, object? old)> onValueChangedSubjectWithOldValue = new Subject<(object? value, object? old)>();
        private object? currentValue;

        public Type Type { get; }
        public string MemberName { get; }
        public Attribute[] Attributes { get; set; }
        public IObservable<object?> OnValueChanged => onValueChangedSubject;
        public IObservable<(object? value, object? old)> OnValueChangedWithOldValue => onValueChangedSubjectWithOldValue;

        /// <summary>
        /// プロパティをラップし、他の要素と同様に <see cref="ValueStorage"/> として扱えるようにします。
        /// </summary>
        /// <param name="subject">プロパティを持っているオブジェクト。</param>
        /// <param name="property">プロパティを表す PropertyInfo。</param>
        public ValueStorage(object subject, PropertyInfo property)
        {
            setter = (value, oldValue) => property.SetValue(subject, value);
            Type = property.PropertyType;
            Attributes = property.GetCustomAttributes().ToArray();
            MemberName = GetMemberName(property.Name, Attributes);

            var get = property.GetValue(subject);
            if (get != null)
            {
                currentValue = get;
            }
            else
            {
                throw new InvalidOperationException("プロパティから値を取得できませんでした。");
            }
        }

        /// <summary>
        /// フィールドをラップし、他の要素と同様に <see cref="ValueStorage"/> として扱えるようにします。
        /// </summary>
        /// <param name="subject">フィールドを持っているオブジェクト。</param>
        /// <param name="field">フィールドを表す FieldInfo。</param>
        public ValueStorage(object subject, FieldInfo field)
        {
            setter = (value, oldValue) => field.SetValue(subject, value);
            Type = field.FieldType;
            Attributes = field.GetCustomAttributes().ToArray();
            MemberName = GetMemberName(field.Name, Attributes);

            var get = field.GetValue(subject);
            if (get != null)
            {
                currentValue = get;
            }
            else
            {
                throw new InvalidOperationException("フィールドから値を取得できませんでした。");
            }
        }

        /// <summary>
        /// デリゲートをラップし、他の要素と同様に <see cref="ValueStorage"/> として扱えるようにします。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName"></param>
        /// <param name="setter">
        ///     値の代入が要求されたときに呼び出されるデリゲート。
        ///     第1引数:設定する値。
        ///     第2引数:インデクサーのインデックスなど、代入に関するその他の情報の配列。
        /// </param>
        /// <param name="getter">
        ///     値の取得が要求されたときに呼び出されるデリゲート。
        ///     第1引数:インデクサーのインデックスなど、取得に関するその他の情報の配列。
        /// </param>
        public ValueStorage(Type type, string memberName, SetterFunction setter, object initialValue)
        {
            this.setter = setter;
            Type = type;
            MemberName = memberName;
            Attributes = new Attribute[0];

            currentValue = initialValue;
        }

        /// <summary>
        /// この要素が表す格納先に値を代入します。
        /// </summary>
        /// <param name="value">設定する値。</param>
        /// <param name="context">インデクサーのインデックスなど、代入先を特定するその他の情報の配列。</param>
        public void SetValue(object? value)
        {
            var oldValue = currentValue;
            setter.Invoke(value, oldValue);
            currentValue = value;

            onValueChangedSubject.OnNext(value);
            onValueChangedSubjectWithOldValue.OnNext((value, oldValue));
        }

        /// <summary>
        /// この要素が表す格納先から値を取得します。
        /// </summary>
        /// <param name="context">インデクサーのインデックスなど、取得元を特定するその他の情報の配列。</param>
        public object? GetValue()
        {
            return currentValue;
        }

        private string GetMemberName(string defaultName, Attribute[] attributes)
        {
            var attr = attributes.OfType<EditorMemberAttribute>().FirstOrDefault();
            return attr?.Title ?? defaultName;
        }
    }
}
