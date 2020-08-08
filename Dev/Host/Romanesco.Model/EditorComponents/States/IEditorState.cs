﻿using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Commands;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.EditorComponents.States
{
	internal interface IEditorState
	{
		string Title { get; }

		IProjectLoadService GetLoadService();
		IProjectSaveService GetSaveService();
		IProjectHistoryService GetHistoryService();

		void OnCreate(IProjectContext project);
		void OnOpen(IProjectContext project);
		void OnSave();
		void OnSaveAs();

		Task<IProjectContext?> CreateAsync();
		Task<IProjectContext?> OpenAsync();
		void NotifyEdit();
		Task SaveAsync();
		Task SaveAsAsync();
		Task ExportAsync();

		// このへんはCommandAvailabilityの責務かも？
		// あるいはEditor側がCommandAvailabilityを使うようにするとよいか？
		// EditorとEditorStateの両方に、CommandAvailabilityを注入するという手がある
		// その場合EditorViewModelにもCommandAvailabilityを注入すれば実装可能そう
		void Undo();
		void Redo();
		void UpdateCanExecute();
	}
}
