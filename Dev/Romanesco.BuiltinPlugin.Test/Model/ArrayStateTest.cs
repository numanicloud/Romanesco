using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Romanesco.BuiltinPlugin.Test.Model
{
    public class ArrayStateTest
    {
        class Project
        {
            public List<int> Array { get; set; }
        }

        [Fact]
        public void 新規要素を追加するとStateが増える()
        {
            var settability = new ValueSettability(typeof(Project).GetProperty("Array"));
            var state = new ListState(settability, s => new IntState(s, new CommandHistory()), new CommandHistory());

            state.AddNewElement();

            Assert.Single(state.Elements);
            Assert.IsType<IntState>(state.Elements[0]);
        }

        [Fact]
        public void 新規要素を追加すると実配列の要素が増える()
        {
            var settability = new ValueSettability(typeof(Project).GetProperty("Array"));
            var state = new ListState(settability, s => new IntState(s, new CommandHistory()), new CommandHistory());

            state.AddNewElement();

            var array = state.ArrayContent.Value;
            Assert.Single(array);
            Assert.IsType<int>(array[0]);
            Assert.Equal(0, array[0]);
        }

        [Fact]
        public void 新規要素の値を書き換えると実配列の中身が書き換わる()
        {
            var settability = new ValueSettability(typeof(Project).GetProperty("Array"));
            var state = new ListState(settability, s => new IntState(s, new CommandHistory()), new CommandHistory());
            var notified = 0;

            var element = state.AddNewElement() as IntState;
            element.Content.Subscribe(x => notified++);
            element.PrimitiveContent.Value = 19;
            element.PrimitiveContent.Value = 23;
            element.PrimitiveContent.Value = 19;

            var array = state.ArrayContent.Value;
            Assert.Equal(4, notified);
            Assert.Equal(19, array[0]);
        }

        [Fact]
        public void 新規要素を追加すると文字列形式が変化する()
        {
            var settability = new ValueSettability(typeof(Project).GetProperty("Array"));
            var state = new ListState(settability, s => new IntState(s, new CommandHistory()), new CommandHistory());

            state.AddNewElement();

            Assert.Equal("Length = 1", state.FormattedString.Value);
        }

        [Fact]
        public void 要素を削除するとStateの要素数が減る()
        {
            var settability = new ValueSettability(typeof(Project).GetProperty("Array"));
            var state = new ListState(settability, s => new IntState(s, new CommandHistory()), new CommandHistory());

            state.AddNewElement();
            state.RemoveAt(0);

            Assert.Empty(state.Elements);
        }

        [Fact]
        public void 要素を削除すると実配列の要素数が減る()
        {
            var settability = new ValueSettability(typeof(Project).GetProperty("Array"));
            var state = new ListState(settability, s => new IntState(s, new CommandHistory()), new CommandHistory());

            state.AddNewElement();
            state.RemoveAt(0);

            Assert.Empty(state.ArrayContent.Value);
        }
    }
}
