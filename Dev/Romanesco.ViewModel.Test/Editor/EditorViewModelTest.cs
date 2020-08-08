using Moq;
using Romanesco.ViewModel.Editor;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using Romanesco.Model.EditorComponents;
using Romanesco.ViewModel.States;
using Romanesco.ViewModel.Test.Helpers;
using Xunit;

namespace Romanesco.ViewModel.Test.Editor
{
	public class EditorViewModelTest
	{
		[Fact]
		public void CreateCommandの実行可能性が反映される()
		{
			SynchronizationContext.SetSynchronizationContext(new TestSynchronizationContext());

			var availability = new Subject<(EditorCommandType type, bool canExecute)>();

			var model = new Mock<IEditorFacade>();
			model.Setup(x => x.CanExecuteObservable)
				.Returns(availability);

			var disposables = new List<IDisposable>();
			model.Setup(x => x.Disposables)
				.Returns(disposables);

			var editor = new EditorViewModel(model.Object, Mock.Of<IViewModelInterpreter>());

			availability.OnNext((EditorCommandType.Create, false));
			Assert.False(editor.CreateCommand.CanExecute());

			availability.OnNext((EditorCommandType.Create, true));
			Assert.True(editor.CreateCommand.CanExecute());
		}
	}
}
