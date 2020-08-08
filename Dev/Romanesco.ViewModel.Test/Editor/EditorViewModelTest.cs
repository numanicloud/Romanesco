using Moq;
using Romanesco.ViewModel.Editor;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using Romanesco.Model.EditorComponents;
using Romanesco.ViewModel.States;
using Xunit;

namespace Romanesco.ViewModel.Test.Editor
{
	public class EditorViewModelTest
	{
		[Fact]
		public void CreateCommandの実行可能性が反映される()
		{
			var availability = new Subject<(EditorCommandType type, bool canExecute)>();

			var model = new Mock<IEditorFacade>();
			model.Setup(x => x.CanExecuteObservable)
				.Returns(availability);

			var editor = new EditorViewModel(model.Object, Mock.Of<ViewModelInterpreter>());

			availability.OnNext((EditorCommandType.Create, false));
			Assert.False(editor.CreateCommand.CanExecute());

			availability.OnNext((EditorCommandType.Create, true));
			Assert.True(editor.CreateCommand.CanExecute());
		}
	}
}
