using Romanesco.Common.Model.Basics;
using Romanesco.Model.Services;

namespace Romanesco.Model.EditorComponents
{
    /// <summary>
    /// エディター全体で用いるコンテキスト。
    /// 編集対象となるプロジェクトが変更されるとコンテキストも新しいものを使用することになる。
    /// </summary>
    class EditorContext
    {
        public Editor Editor { get; }
        public IProjectSettingProvider SettingProvider { get; }
		public ProjectContext CurrentProject { get; }

		public EditorContext(Editor editor, IProjectSettingProvider settingProvider,
            ProjectContext currentProject)
        {
            Editor = editor;
            SettingProvider = settingProvider;
            CurrentProject = currentProject;
        }
    }
}
