using System;
using System.Linq;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using Romanesco.ViewModel.Infrastructure;

namespace Romanesco.ViewModel.Project
{
	internal class VmProjectSettingsProvider : IProjectSettingProvider
    {
	    private readonly IViewModelFactory factory;

		public VmProjectSettingsProvider(IViewModelFactory factory)
		{
			this.factory = factory;
		}

        public ProjectSettings? InputCreateSettings(ProjectSettingsEditor editor)
        {
            factory.ResolveEditorViewModel().ShowProjectSetting(editor);

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
