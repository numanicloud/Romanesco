using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Xunit;

namespace Romanesco.BuiltinPlugin.Test.Model
{
	public class ClassTest
	{
		private class Hoge
		{
			public int X { get; set; }

			public override string ToString()
			{
				return $"Hoge: X = {X}";
			}
		}

		private PropertyInfo GetPropertyInfo(string name)
		{
			return typeof(Hoge).GetProperty(name)!;
		}

		private (ClassState, IntState) CreateClassState()
		{
			var hoge = new Hoge();
			var dummyStorage = new ValueStorage(typeof(Hoge), "Root", (value, oldValue) => { }, hoge);
			var storage = new ValueStorage(hoge, GetPropertyInfo(nameof(Hoge.X)));
			var intField = new IntState(storage, new CommandHistory());
			return (new ClassState(dummyStorage, new[] { intField }), intField);
		}

		[Fact]
		public void 子要素を編集するとOnEditedが発火する()
		{
			var (state, field) = CreateClassState();
			var edited = 0;
			state.OnEdited.Subscribe(x => edited++);

			field.Storage.SetValue(12);
			Assert.Equal(1, edited);

			field.Storage.SetValue(39);
			Assert.Equal(2, edited);
		}

		[Fact]
		public void 子要素を編集するとFormattedStringが変更される()
		{
			var sampleHoge1 = new Hoge() { X = 99 };
			var sampleHoge2 = new Hoge() { X = 101 };
			var (state, field) = CreateClassState();

			field.Storage.SetValue(99);
			Assert.Equal(sampleHoge1.ToString(), state.FormattedString.Value);
			field.Storage.SetValue(101);
			Assert.Equal(sampleHoge2.ToString(), state.FormattedString.Value);
		}
	}
}
