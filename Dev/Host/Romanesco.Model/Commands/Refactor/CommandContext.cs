using System.Collections.Generic;
using System.Threading.Tasks;
using Romanesco.Common.Model.Helpers;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.Infrastructure;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.Model.Commands.Refactor;

public class CommandContext
{
	private readonly IEditorStateChanger _stateChanger;
	private readonly Dictionary<EditorCommandType, CommandModelRefactor> _commands = new();
	private readonly BooleanUsingScopeSource _usingScope = new ();

	internal CommandContext(IProjectSwitcher projectSwitcher,
		IEditorStateChanger stateChanger,
		IModelFactory factory)
	{
		_stateChanger = stateChanger;
		_commands = new Dictionary<EditorCommandType, CommandModelRefactor>()
		{
			[Create] = new CreateCommand(projectSwitcher, stateChanger, factory),
			[Open] = new LoadCommand(projectSwitcher, stateChanger, factory),
			[Save] = new SaveCommand(projectSwitcher, stateChanger, factory),
			[SaveAs] = new SaveAsCommand(projectSwitcher, stateChanger, factory),
			[Export] = new ExportCommand(),
			[Undo] = new UndoCommand(),
			[Redo] = new RedoCommand(),
		};

		UpdateCanExecute();
	}

	public bool CanExecute(EditorCommandType type)
	{
		return _commands[type].CanExecute.Value;
	}

	public async Task ExecuteAsync(EditorCommandType type)
	{
		if (_usingScope.IsUsing.Value)
		{
			return;
		}

		using (_usingScope.Create())
		{
			await _commands[type].Execute(_stateChanger.GetCurrent());
		}

		AfterExecution(type);
	}

	public void UpdateCanExecute()
	{
		var load = _stateChanger.GetCurrent().GetLoadService();
		_commands[Create].CanExecute.Value = load.CanCreate;
		_commands[Open].CanExecute.Value = load.CanOpen;

		var save = _stateChanger.GetCurrent().GetSaveService();
		_commands[Save].CanExecute.Value = save.CanSave;
		_commands[SaveAs].CanExecute.Value = save.CanSave;
		_commands[Export].CanExecute.Value = save.CanExport;

		var history = _stateChanger.GetCurrent().GetHistoryService();
		_commands[Undo].CanExecute.Value = history.CanUndo;
		_commands[Redo].CanExecute.Value = history.CanRedo;
	}

	private void AfterExecution(EditorCommandType type)
	{
		_commands[type].AfterExecute(_stateChanger.GetCurrent());
		UpdateCanExecute();
	}
}
