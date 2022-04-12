using Romanesco.Common.Model.Basics;

namespace Romanesco.Model.Services.History
{
	internal sealed class SimpleHistoryService : IProjectHistoryService
	{
		private readonly CommandHistory commandHistory;

		public SimpleHistoryService(CommandHistory commandHistory)
		{
			this.commandHistory = commandHistory;
		}

		public bool CanUndo => commandHistory.CanUndo;

		public bool CanRedo => commandHistory.CanRedo;

		public void Redo()
		{
			commandHistory.Redo();
		}

		public void Undo()
		{
			commandHistory.Undo();
		}
	}
}
