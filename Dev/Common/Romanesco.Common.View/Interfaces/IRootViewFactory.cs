using System.Windows.Controls;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.Common.View.Interfaces;

public interface IRootViewFactory
{
	UserControl? Interpret(IStateViewModel viewModel);
}