using Microsoft.CSharp.RuntimeBinder;
using Romanesco.Common.Model.Basics;
using Xunit;

namespace Romanesco.Test
{
	public class DynamicMockTest
	{
		class Hoge
		{
			public int X { get; set; }
			public int Y { get; set; }
			public int U;
			public int V;
		}

		[Fact]
		public void モックに設定したプロパティにgetsetできる()
		{
			var props = typeof(Hoge).GetProperties();
			dynamic mock = new DynamicMock(props, new System.Reflection.FieldInfo[0]);

			mock["X"] = 12;
			mock["Y"] = 99;

			Assert.Equal(12, mock["X"]);
			Assert.Equal(99, mock["Y"]);
		}

		[Fact]
		public void モックに設定していないプロパティにgetsetできない()
		{
			var props = typeof(Hoge).GetProperties();
			dynamic mock = new DynamicMock(props, new System.Reflection.FieldInfo[0]);

			Assert.Throws<RuntimeBinderException>(() =>
			{
				mock["Q"] = 12;
			});
		}

		[Fact]
		public void モックに設定したフィールドにgetsetできる()
		{
			var fields = typeof(Hoge).GetFields();
			dynamic mock = new DynamicMock(new System.Reflection.PropertyInfo[0], fields);

			mock["U"] = 81;
			mock["V"] = 11;

			Assert.Equal(81, mock["U"]);
			Assert.Equal(11, mock["V"]);
		}

		[Fact]
		public void モックに設定していないフィールドにgetsetできない()
		{
			var fields = typeof(Hoge).GetFields();
			dynamic mock = new DynamicMock(new System.Reflection.PropertyInfo[0], fields);

			Assert.Throws<RuntimeBinderException>(() =>
			{
				mock["H"] = 77;
			});
		}

		[Fact]
		public void モックで取得したものから型を取れる()
		{
			var fields = typeof(Hoge).GetFields();
			dynamic mock = new DynamicMock(new System.Reflection.PropertyInfo[0], fields);

			Assert.Equal(typeof(int), mock["Type:U"]);
		}
	}
}
