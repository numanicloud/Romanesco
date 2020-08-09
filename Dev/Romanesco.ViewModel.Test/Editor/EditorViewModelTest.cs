using Moq;
using Romanesco.ViewModel.Editor;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.ViewModel.States;
using Romanesco.ViewModel.Test.Helpers;
using Xunit;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.ViewModel.Test.Editor
{
	public class EditorViewModelTest
	{
		[Theory]
		[InlineData(Create)]
		[InlineData(Open)]
		[InlineData(Save)]
		[InlineData(SaveAs)]
		[InlineData(Export)]
		[InlineData(Undo)]
		[InlineData(Redo)]
		public void CreateCommandの実行可能性が反映される(EditorCommandType type)
		{
			SynchronizationContext.SetSynchronizationContext(new TestSynchronizationContext());

			var availability = new Subject<(EditorCommandType type, bool canExecute)>();
			var commands = new CommandAvailability(Mock.Of<IEditorState>());

			var model = new Mock<IEditorFacade>();
			model.Setup(x => x.CanExecuteObservable)
				.Returns(availability);
			model.Setup(x => x.CommandAvailabilityPublisher)
				.Returns(commands);

			var disposables = new List<IDisposable>();
			model.Setup(x => x.Disposables)
				.Returns(disposables);

			var editor = new EditorViewModel(model.Object, Mock.Of<IViewModelInterpreter>());

			var targetCommand = type switch
			{
				Create => editor.CreateCommand,
				Open => editor.OpenCommand,
				Save => editor.SaveCommand,
				SaveAs => editor.SaveAsCommand,
				Export => editor.ExportCommand,
				Undo => editor.Undo,
				Redo => editor.Redo,
				_ => throw new NotImplementedException(),
			};

			//availability.OnNext((type, false));
			commands.UpdateCanExecute(type, false);
			Assert.False(targetCommand.CanExecute());

			//availability.OnNext((type, true));
			commands.UpdateCanExecute(type, true);
			Assert.True(targetCommand.CanExecute());
		}
	}
}
