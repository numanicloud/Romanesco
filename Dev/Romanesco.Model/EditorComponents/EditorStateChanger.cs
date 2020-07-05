using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Save;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Subjects;
using Romanesco.Common.Model.ProjectComponent;

namespace Romanesco.Model.EditorComponents
{
	internal class EditorStateChanger
	{
		private readonly Subject<EditorState> onChangeSubject = new Subject<EditorState>();
		private readonly IServiceLocator serviceLocator;
		private readonly ProjectSaveServiceFactory saveServiceFactory;

		// TODO: 持ちまわしている ProjectContext が null かどうか、同一かどうかなどが隠蔽されていて分かりづらい
		private ProjectContext? currentProjectContext;

		public IObservable<EditorState> OnChange => onChangeSubject;

		public EditorStateChanger(IServiceLocator serviceLocator, ProjectSaveServiceFactory saveServiceFactory)
		{
			this.serviceLocator = serviceLocator;
			this.saveServiceFactory = saveServiceFactory;
		}

		public void InitializeState([NotNull]ref EditorState store)
		{
			var state = serviceLocator.CreateInstance<EmptyEditorState>(this);
			store = state;
			onChangeSubject.OnNext(state);
		}

		public void ChangeToNew(ProjectContext context)
		{
			currentProjectContext?.Project.Root.Dispose();
			currentProjectContext = context;

			var save = saveServiceFactory.Create(currentProjectContext);
			var state = serviceLocator.CreateInstance<NewEditorState>(save, this);
			onChangeSubject.OnNext(state);
		}

		public void ChangeToClean(ProjectContext? context = null)
		{
			currentProjectContext?.Project.Root.Dispose();
			if (context is { })
			{
				currentProjectContext = context;
			}
			if (currentProjectContext is null)
			{
				throw new InvalidOperationException("開かれているプロジェクトがありません。");
			}

			var save = saveServiceFactory.Create(currentProjectContext);
			var state = serviceLocator.CreateInstance<CleanEditorState>(save, currentProjectContext, this);
			onChangeSubject.OnNext(state);
		}

		public void ChangeToDirty()
		{
			if (currentProjectContext is null)
			{
				throw new InvalidOperationException("開かれているプロジェクトがありません。");
			}

			var save = saveServiceFactory.Create(currentProjectContext);
			var state = serviceLocator.CreateInstance<DirtyEditorState>(save, currentProjectContext, this);
			onChangeSubject.OnNext(state);
		}
	}
}
