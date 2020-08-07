using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.Infrastructure;
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

		// 怠けでvirtualにしているが、このクラスに更に基底の IEditorState とかが必要かもしれない
		Task<IProjectContext?> CreateAsync();

		void OnCreate(IProjectContext project);
		void OnOpen(IProjectContext project);
		void OnSave();
		void OnSaveAs();
		void OnExport();
		void OnUndo();
		void OnRedo();
		void OnEdit();

		void Undo(IObserver<(EditorCommandType, bool)> observer);
		void UpdateHistoryAvailability(IObserver<(EditorCommandType, bool)> observer);
		void UpdateCanExecute(IObserver<(EditorCommandType, bool)> observer);
	}
}
