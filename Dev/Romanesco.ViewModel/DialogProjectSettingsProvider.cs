using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Romanesco.ViewModel
{
    public class VmProjectSettingsProvider : IProjectSettingProvider
    {
        public EditorViewModel ViewModel { get; set; }

        public void InputCreateSettings(ProjectSettingsEditor editor)
        {
            ViewModel.ShowProjectSetting(editor);
        }
    }
}
