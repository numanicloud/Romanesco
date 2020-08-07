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
		public Task<ProjectContext?> CreateAsync();

		public void OnCreate(ProjectContext project);

		public void OnOpen(ProjectContext project);

		public void OnSave();

		public void OnSaveAs();

		public void OnExport();

		public void OnUndo();

		public void OnRedo();

		public void OnEdit();
	}
}
