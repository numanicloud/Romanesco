using System.Windows.Controls;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.View.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.View.Factories;

public class RootViewFactory : IRootViewFactory
{
	public UserControl? Interpret(IStateViewModel viewModel)
	{
		return viewModel switch
		{
			IntViewModel @int => new View2.TextView() { DataContext = @int },
			ShortViewModel @short => new View2.TextView() { DataContext = @short },
			ByteViewModel @byte => new View2.TextView() { DataContext = @byte },
			LongViewModel @long => new View2.TextView() { DataContext = @long },
			FloatViewModel @float => new View2.TextView() { DataContext = @float },
			DoubleViewModel @double => new View2.TextView() { DataContext = @double },
			BoolViewModel @bool => new View2.CheckboxView() { DataContext = @bool },
			EnumViewModel @enum => new View2.EnumView() { DataContext = @enum },
			StringViewModel @string => new View2.TextView() { DataContext = @string },
			ClassViewModel @class => new View2.ClassBlockView() { DataContext = @class },
			SubtypingClassViewModel subtyping => new View2.SubtypingBlockView() { DataContext = subtyping },
			IntIdChoiceViewModel id => new View2.IdChoiceView() { DataContext = id },
			ListViewModel list => new View2.ListBlockView() { DataContext = list },
			PrimitiveListViewModel primitiveList => new View2.PrimitiveListBlockView() { DataContext = primitiveList },
			IntIdChoiceListViewModel idList => new View2.IdChoiceListView() { DataContext = idList },
			_ => null
		};
	}
}