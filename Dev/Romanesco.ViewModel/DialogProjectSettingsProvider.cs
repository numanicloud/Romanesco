using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romanesco.ViewModel
{
    public class VmProjectSettingsProvider : IProjectSettingProvider
    {
        public EditorViewModel? ViewModel { get; set; }

        public ProjectSettings? InputCreateSettings(ProjectSettingsEditor editor)
        {
            ViewModel?.ShowProjectSetting(editor);

            if (editor.Succeeded)
            {
                if (editor.ProjectType is null || editor.ExporterType is null)
                {
                    throw new InvalidOperationException("プロジェクト設定、またはプラグイン構成が無効です。");
                }
                else
                {
                    return new ProjectSettings(
                        editor.Assembly,
                        editor.ProjectType,
                        editor.ExporterType,
                        editor.DependencyProjects.ToArray());
                }
            }
            else
            {
                return null;
            }
        }
    }
}
