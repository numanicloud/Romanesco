using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Romanesco.ViewModel.Commands
{
	class SaveCommandViewModel : ICommand
	{
		public event EventHandler? CanExecuteChanged;

		public SaveCommandViewModel()
		{
			
		}

		public bool CanExecute(object parameter)
		{
			throw new NotImplementedException();
		}

		public void Execute(object parameter)
		{
			throw new NotImplementedException();
		}
	}
}
