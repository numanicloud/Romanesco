﻿using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Romanesco.Sample
{
    class SampleProjectSettingProvider : IProjectSettingProvider
    {
        public void InputCreateSettings(ProjectSettingsEditor editor)
        {
            editor.AssemblyPath.Value = typeof(Project).Assembly.Location;
            editor.ProjectTypeFullName.Value = typeof(Project).FullName;
        }
    }
}
