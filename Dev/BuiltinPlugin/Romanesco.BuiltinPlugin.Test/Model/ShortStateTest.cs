using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Xunit;

namespace Romanesco.BuiltinPlugin.Test.Model
{
	public class ShortStateTest
	{
		[Fact]
		public void 値を更新するとFormattedStringに反映される()
		{
			var storage = new ValueStorage(typeof(short), "Value", (value, oldValue) => { }, (short)0);
			var state = new ShortState(storage, new CommandHistory());

			Assert.Equal(((short)0).ToString(), state.FormattedString.Value);

			state.PrimitiveContent.Value = 99;

			Assert.Equal(((short)99).ToString(), state.FormattedString.Value);
		}
	}
}
