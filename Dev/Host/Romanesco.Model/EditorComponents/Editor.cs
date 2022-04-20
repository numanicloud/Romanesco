using Reactive.Bindings;
using Romanesco.Model.EditorComponents.States;
using System;
using System.Collections.Generic;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands.Refactor;

namespace Romanesco.Model.EditorComponents
{
	internal sealed class Editor : IEditorFacade, IDisposable
	{
		private readonly ReactiveProperty<IEditorState> _editorState = new();

		public List<IDisposable> Disposables { get; } = new();
		public ReactiveProperty<string> ApplicationTitle { get; } = new();
		public CommandContext CommandContext { get; }
		public IProjectSwitcher ProjectSwitcher { get; }

		public ReactiveProperty<IFieldState[]> Roots { get; }

		public Editor
			(IProjectSwitcher switcher,
			IEditorState initialState,
			CommandContext commands)
		{
			ProjectSwitcher = switcher;
			CommandContext = commands;
			Roots = new ReactiveProperty<IFieldState[]>(ProjectSwitcher.GetProject()?.StateRoot.States ?? Array.Empty<IFieldState>());

			// commandAvailability の初期化を保証しなければならないので、
			// ChangeStateをインライン化した
			_editorState.Value = initialState;
			UpdateTitle();
			
			SetUpCommand();
		}

		private void SetUpCommand()
		{
			ProjectSwitcher.ProjectStream.Subscribe(SetProject);
		}

		private void SetProject(IProjectContext? projectContext)
		{
			UpdateTitle();

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
					CommandContext.UpdateCanExecute();
				})
				.AddTo(Disposables);
		}

		private void UpdateTitle() => ApplicationTitle.Value = _editorState.Value.Title;

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
