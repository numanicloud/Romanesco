using System.Collections.Generic;
using System.Threading.Tasks;
using Romanesco.Common.Model.Helpers;
using Romanesco.Model.Infrastructure;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.States;
using static Romanesco.Model.Commands.EditorCommandType;

namespace Romanesco.Model.Commands;

public class CommandContext
{
	private readonly IEditorStateChanger _stateChanger;
	private readonly Dictionary<EditorCommandType, RomanescoCommand> _commands = new();
	private readonly BooleanUsingScopeSource _usingScope = new ();

	public IReadOnlyDictionary<EditorCommandType, RomanescoCommand> Commands => _commands;

	internal CommandContext(IProjectSwitcher projectSwitcher,
		IEditorStateChanger stateChanger,
		IModelFactory factory)
	{
		_stateChanger = stateChanger;
		_commands = new Dictionary<EditorCommandType, RomanescoCommand>()
		{
			[Create] = new CreateRomanescoCommand(projectSwitcher, stateChanger, factory),
			[Open] = new LoadRomanescoCommand(projectSwitcher, stateChanger, factory),
			[Save] = new SaveRomanescoCommand(projectSwitcher, stateChanger, factory),
			[SaveAs] = new SaveAsRomanescoCommand(projectSwitcher, stateChanger, factory),
			[Export] = new ExportRomanescoCommand(),
			[Undo] = new UndoRomanescoCommand(),
			[Redo] = new RedoRomanescoCommand(),
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
