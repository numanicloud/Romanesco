using System;

namespace Romanesco.Common.Model.ValueStorages
{
	/// <summary>
	/// プロパティなどに値を代入する操作を表すデリゲート型。
	/// </summary>
	/// <param name="value">これから代入する値。</param>
	/// <param name="oldValue">操作を行う前に代入されていた値。</param>
	public delegate void SetterFunction(object? value, object? oldValue);

	/// <summary>
	/// プロパティ、フィールドというような、何らかの値を代入できるものを表します。
	/// </summary>
	public record ValueStorage(Type Type, SetterFunction Setter)
	{
		private object? _value;

		/// <summary>
		/// 現在の値を取得または設定します。
		/// </summary>
		public object? Value
		{
			get => _value;
			set
			{
				Setter.Invoke(value, _value);
				_value = value;
			}
		}
	}
}
