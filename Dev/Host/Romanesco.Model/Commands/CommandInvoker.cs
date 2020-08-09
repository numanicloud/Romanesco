using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Interfaces;

namespace Romanesco.Model.Commands
{
	/*
	 * コマンドのいろいろな姿
	 * - Editor.CreateAsync: コマンド実行。EditorStateはEditorが与えてくれる。
	 * - CommandAvailability.CanCreate: 実行可能性。
	 * - CommandAvailability.CreateAsync: コマンド実行。IEditorStateを与える必要がある。
	 * - EditorViewModel.ToReactiveCommand: コマンド化。
	 * - EditorViewModel.Subscribe: ビューモデル側の処理を行い、その途中でモデルの処理を利用する。
	 *
	 * CommandInvokerにクラスを分けるのは関心事がまとまっていなさすぎるかも。
	 * CommandAvailability自体を IEditorState ごとに作成するものと考えるべきかも
	 */

	class CommandInvoker : ICommandInvoker
	{
		private readonly IEditorState currentState;

		public CommandInvoker(IEditorState currentState)
		{
			this.currentState = currentState;
		}

		public async Task<IProjectContext?> CreateAsync()
		{
			return await currentState.GetLoadService().CreateAsync();
		}

		public async Task<IProjectContext?> OpenAsync()
		{
			return await currentState.GetLoadService().OpenAsync();
		}

		public async Task SaveAsync()
		{
			await currentState.GetSaveService().SaveAsync();
			currentState.OnSave();
		}

		public async Task SaveAsAsync()
		{
			await currentState.GetSaveService().SaveAsAsync();
			currentState.OnSaveAs();
		}

		public async Task ExportAsync()
		{
			await currentState.GetSaveService().ExportAsync();
		}

		public void Undo()
		{
			currentState.GetHistoryService().Undo();
			//UpdateCanExecute(EditorCommandType.Undo, currentState.GetHistoryService().CanUndo);
		}

		public void Redo()
		{
			currentState.GetHistoryService().Redo();
			//UpdateCanExecute(EditorCommandType.Redo, currentState.GetHistoryService().CanRedo);
		}
	}
}
