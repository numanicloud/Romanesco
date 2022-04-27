using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.Commands.Refactor;

namespace Romanesco.Model.EditorComponents
{
	internal sealed class Editor : IEditorFacade, IDisposable
	{
		public List<IDisposable> Disposables { get; } = new();
		public ReactiveProperty<string> ApplicationTitle { get; }
		public CommandContext CommandContext { get; }
		public IProjectSwitcher ProjectSwitcher { get; }

		public ReactiveProperty<IFieldState[]> Roots { get; }

		public Editor
			(IProjectSwitcher switcher,
			IEditorStateChanger stateChanger,
			CommandContext commands)
		{
			ProjectSwitcher = switcher;
			CommandContext = commands;
			Roots = new ReactiveProperty<IFieldState[]>(ProjectSwitcher.GetProject()?.StateRoot.States ?? Array.Empty<IFieldState>());
			
			ApplicationTitle = stateChanger.OnChange
				.Select(x => x.Title)
				.ToReactiveProperty("");

			SetUpCommand();
		}

		private void SetUpCommand()
		{
			ProjectSwitcher.ProjectStream.Subscribe(SetProject);
		}

		private void SetProject(IProjectContext? projectContext)
		{
			Roots.Value = ProjectSwitcher.GetProject()?.StateRoot.States ?? Array.Empty<IFieldState>();

			ObserveEdit(projectContext);
		}

		private void ObserveEdit(IProjectContext? projectContext)
		{
			if (projectContext is null)
			{
				return;
			}

			projectContext.ObserveEdit(() =>
				{
					// Undo, Redoができるかの状態を更新するために
					CommandContext.UpdateCanExecute();
				})
				.AddTo(Disposables);
		}

		public void Dispose()
		{
			ApplicationTitle.Dispose();
			foreach (var disposable in Disposables)
			{
				disposable.Dispose();
			}
		}
	}
}
