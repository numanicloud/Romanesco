using System;
using System.Collections;
using System.Reflection;

namespace Romanesco.Common.Model.ValueStorages
{
	/// <summary>
	/// リフレクション情報などに基づいて、<see cref="ValueStorage"/> のインスタンスを生成するファクトリー クラス。
	/// </summary>
	public static class ValueStorageFactory
	{
		/// <summary>
		/// プロパティ情報に基づいて、 <see cref="ValueStorage"/> の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="property">プロパティのリフレクション情報。</param>
		/// <param name="declaringInstance">プロパティへget/set操作が行われる際に、プロパティの持ち主として扱うオブジェクト。</param>
		/// <returns></returns>
		public static ValueStorage ToValueStorage(this PropertyInfo property, object declaringInstance)
		{
			void Setter(object? value, object? _) => property.SetValue(declaringInstance, value);

			return new(property.PropertyType, Setter)
			{
				Value = property.GetValue(declaringInstance)
			};
		}

		/// <summary>
		/// フィールド情報に基づいて、 <see cref="ValueStorage"/> の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="field">フィールドのリフレクション情報。</param>
		/// <param name="declaringInstance">フィールドへget/set操作が行われる際に、フィールドの持ち主として扱うオブジェクト。</param>
		/// <returns></returns>
		public static ValueStorage ToValueStorage(this FieldInfo field, object declaringInstance)
		{
			void Setter(object? value, object? _) => field.SetValue(declaringInstance, value);

			return new(field.FieldType, Setter)
			{
				Value = field.GetValue(declaringInstance)
			};
		}

		/// <summary>
		/// リスト内の要素に対するget/setを隠蔽するような <see cref="ValueStorage"/> の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="elementType">リスト要素の型。</param>
		/// <param name="list">リストのインスタンス。</param>
		/// <param name="initialValue">この要素の初期値。</param>
		/// <returns></returns>
		public static ValueStorage FromListElement(Type elementType, IList list, object? initialValue)
		{
			void Setter(object? value, object? oldValue)
			{
				var i = list.IndexOf(oldValue);
				if (i != -1)
				{
					list[i] = value;
				}
			}

			return new(elementType, Setter)
			{
				Value = initialValue
			};
		}
	}
}
