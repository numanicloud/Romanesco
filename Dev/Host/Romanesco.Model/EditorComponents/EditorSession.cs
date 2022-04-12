using System;
using System.Collections.Generic;
using System.Text;
using Romanesco.Model.Commands;

namespace Romanesco.Model.EditorComponents
{
	/// <summary>
	/// 表示されているひとつのエディター画面に対応するコンテキストクラス。
	/// </summary>
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
