using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Model.Commands;

namespace Romanesco.Model.EditorComponents
{
	class EditorSession
	{
		public IEditorStateChanger EditorStateChanger { get; }
		public CommandAvailability CommandAvailability { get; }

		public EditorSession(IEditorStateChanger editorStateChanger, CommandAvailability commandAvailability)
		{
			EditorStateChanger = editorStateChanger;
			CommandAvailability = commandAvailability;
		}
	}
}
