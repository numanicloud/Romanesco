using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Xunit;

namespace Romanesco.BuiltinPlugin.Test.Model;

public class IntStateTest
{
	[Fact]
	public void 値を更新するとFormattedStringが更新される()
	{
		var storage = new ValueStorage(typeof(int), "Hoge", (value, oldValue) => {}, 0);
		var state = new IntState(storage, new CommandHistory());

		Assert.Equal(0.ToString(), state.FormattedString.Value);
		
		state.PrimitiveContent.Value = 12;

		Assert.Equal(12.ToString(), state.FormattedString.Value);
	}
}