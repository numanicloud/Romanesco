using System;
using System.Windows.Input;
using Romanesco.Common.Model.Helpers;
using Romanesco.Model.Commands.Refactor;
using Romanesco.Model.EditorComponents;

namespace Romanesco.ViewModel.Editor;

internal class EditorCommand : ICommand, IDisposable
{
	private readonly CommandContext _commands;
	private readonly EditorCommandType _type;
	private readonly IDisposable _canExecuteSubscription;

	public EditorCommand(CommandContext commands, EditorCommandType type)
	{
		_commands = commands;
		_type = type;

		_canExecuteSubscription = commands.Commands[type].CanExecute
			.Subscribe(x => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
	}

	public bool CanExecute(object? parameter)
	{
		return _commands.CanExecute(_type);
	}

	public virtual async void Execute(object? parameter)
	{
		await _commands.ExecuteAsync(_type);
		CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}

	public event EventHandler? CanExecuteChanged;

	public void Dispose()
	{
		_canExecuteSubscription.Dispose();
	}
}
