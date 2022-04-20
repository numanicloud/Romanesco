using System;
using System.Windows.Input;
using Romanesco.Common.Model.Helpers;
using Romanesco.Model.Commands.Refactor;
using Romanesco.Model.EditorComponents;

namespace Romanesco.ViewModel.Editor;

internal class EditorCommand : ICommand
{
	private readonly CommandContext _commands;
	private readonly EditorCommandType _type;

	public EditorCommand(CommandContext commands, EditorCommandType type)
	{
		_commands = commands;
		_type = type;
	}

	public bool CanExecute(object? parameter)
	{
		return _commands.CanExecute(_type);
	}

	public virtual void Execute(object? parameter)
	{
		_commands.ExecuteAsync(_type).Forget();
		CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}

	public event EventHandler? CanExecuteChanged;
}
