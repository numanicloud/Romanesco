using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Livet.Messaging;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.ViewModel.Project;
using Romanesco.ViewModel.States;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.ViewModel.Editor
{
	internal class EditorViewModel : Livet.ViewModel, IEditorViewModel
	{
		private readonly Dictionary<EditorCommandType, EditorCommand> _commands;

		public IEditorFacade Editor { get; }
		public ReactiveProperty<IStateViewModel[]> Roots { get; }
		public ICommand CreateCommand => _commands[Create];
		public ICommand OpenCommand => _commands[Open];
		public ICommand SaveCommand => _commands[Save];
		public ICommand SaveAsCommand => _commands[SaveAs];
		public ICommand ExportCommand => _commands[Export];
		public ICommand UndoCommand => _commands[Undo];
		public ICommand RedoCommand => _commands[Redo];
		public ReactiveCommand GcDebugCommand { get; } = new ReactiveCommand();
		public List<IDisposable> Disposables { get; }

		public EditorViewModel(IEditorFacade editor, IViewModelInterpreter interpreter)
		{
			Editor = editor;
			Roots = editor.Roots
				.Select(x => x.Select(interpreter.InterpretAsViewModel).ToArray())
				.ToReactiveProperty(Array.Empty<IStateViewModel>());

			_commands = new Dictionary<EditorCommandType, EditorCommand>()
			{
				[Create] = new(editor.CommandContext, Create),
				[Save] = new(editor.CommandContext, Save),
				[Open] = new(editor.CommandContext, Open),
				[SaveAs] = new(editor.CommandContext, SaveAs),
				[Export] = new(editor.CommandContext, Export),
				[Undo] = new(editor.CommandContext, Undo),
				[Redo] = new(editor.CommandContext, Redo),
			};

			Disposables = editor.Disposables;
			GcDebugCommand.SubscribeSafe(x => GC.Collect()).AddTo(editor.Disposables);

			_commands.ForEach(t => Disposables.Add(t.Value));
		}

		public void ShowProjectSetting(ProjectSettingsEditor editor)
		{
			var vm = new ProjectSettingsEditorViewModel(editor);
			Messenger.Raise(new TransitionMessage(vm, "CreateProject"));
		}
	}
}
