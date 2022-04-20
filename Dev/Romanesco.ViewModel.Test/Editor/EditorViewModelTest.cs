using Moq;
using Romanesco.ViewModel.Editor;
using System;
using System.Collections.Generic;
using System.Threading;
using Reactive.Bindings;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Interfaces;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Test.Helpers;
using Romanesco.ViewModel.States;
using Romanesco.ViewModel.Test.Helpers;
using Xunit;
using static Romanesco.Model.EditorComponents.EditorCommandType;

namespace Romanesco.ViewModel.Test.Editor
{
	public class EditorViewModelTest
	{

		private static Mock<IEditorFacade> GetEditorModel()
		{
			var disposables = new List<IDisposable>();
			var model = new Mock<IEditorFacade>();
			model.Setup(x => x.Disposables)
				.Returns(disposables);
			return model;
		}

		private static Mock<ICommandAvailabilityPublisher> GetCommands()
		{
			var commands = new Mock<ICommandAvailabilityPublisher>();
			commands.Setup(x => x.CanCreate).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanOpen).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanSave).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanSaveAs).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanExport).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanUndo).Returns(new ReactiveProperty<bool>(true));
			commands.Setup(x => x.CanRedo).Returns(new ReactiveProperty<bool>(true));
			return commands;
		}
	}
}
