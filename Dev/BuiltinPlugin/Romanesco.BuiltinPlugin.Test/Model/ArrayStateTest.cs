using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Romanesco.BuiltinPlugin.Model.Factories;
using Xunit;

namespace Romanesco.BuiltinPlugin.Test.Model
{
    public class ArrayStateTest
    {
	    private class Project
        {
            public List<int> Array { get; set; } = new List<int>();
        }

        private PropertyInfo GetProperty(string propertyName)
        {
            return typeof(Project).GetProperty(propertyName) ?? throw new Exception();
        }

        private ListState GetIntArrayState(string propertyName)
        {
            var project = new Project();
	        var storage = new ValueStorage(project, GetProperty("Array"));
	        var list = new List<int>();
	        storage.SetValue(list);
	        return new ListState(storage, list, s => new IntState(s, new CommandHistory()), new CommandHistory());
        }

        [Fact]
        public void 新規要素を追加するとStateが増える()
        {
	        var state = GetIntArrayState(nameof(Project.Array));
            state.AddNewElement();

            Assert.Single(state.Elements);
            Assert.IsType<IntState>(state.Elements[0]);
        }

        [Fact]
        public void 新規要素を追加すると実配列の要素が増える()
        {
	        var state = GetIntArrayState(nameof(Project.Array));
            state.AddNewElement();

            var array = (state.Storage.GetValue() as IList)!;
            Assert.Single(array);
            Assert.IsType<int>(array[0]);
            Assert.Equal(0, array[0]);
        }

        [Fact]
        public void 新規要素の値を書き換えると実配列の中身が書き換わる()
        {
	        var state = GetIntArrayState(nameof(Project.Array));
            var notified = 0;

            var element = (state.AddNewElement() as IntState)!;
            element.PrimitiveContent.Subscribe(x => notified++);
            element.PrimitiveContent.Value = 19;
            element.PrimitiveContent.Value = 23;
            element.PrimitiveContent.Value = 19;

            var array = (state.Storage.GetValue() as IList)!;
            Assert.Equal(4, notified);
            Assert.Equal(19, array[0]);
        }

        [Fact]
        public void 新規要素を追加すると文字列形式が変化する()
        {
	        var state = GetIntArrayState(nameof(Project.Array));
            state.AddNewElement();

            Assert.Equal("Count = 1", state.FormattedString.Value);
        }

        [Fact]
        public void 要素を削除するとStateの要素数が減る()
        {
	        var state = GetIntArrayState(nameof(Project.Array));
            state.AddNewElement();
            state.RemoveAt(0);

            Assert.Empty(state.Elements);
        }

        [Fact]
        public void 要素を削除すると実配列の要素数が減る()
        {
	        var state = GetIntArrayState(nameof(Project.Array));
            state.AddNewElement();
            state.RemoveAt(0);

            Assert.Empty(state.Storage.GetValue() as IList);
        }
    }
}
