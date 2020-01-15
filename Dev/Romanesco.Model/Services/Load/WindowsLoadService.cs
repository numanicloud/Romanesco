﻿using Microsoft.Win32;
using Newtonsoft.Json;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.Serialize;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Romanesco.Model.Services.Load
{
    class WindowsLoadService : IProjectLoadService
    {
        private readonly EditorContext context;
        private readonly IStateDeserializer deserializer;

        public WindowsLoadService(EditorContext context, IStateDeserializer deserializer)
        {
            this.context = context;
            this.deserializer = deserializer;
        }

        public bool CanCreate => true;

        public bool CanOpen => true;

        public Project Create()
        {
            var editor = new ProjectSettingsEditor();
            context.SettingProvider.InputCreateSettings(editor);
            if (editor.Succeeded)
            {
                var settings = new ProjectSettings(editor.Assembly, editor.ProjectType);
                var instance = Activator.CreateInstance(settings.ProjectType);
                return ProjectConverter.FromInstance(settings, context.Interpreter, instance);
            }
            return null;
        }

        public async Task<Project> OpenAsync()
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "マスター プロジェクト (*.roma)|*.roma",
                Title = "マスター プロジェクトを読み込む"
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                using (var file = File.OpenRead(dialog.FileName))
                {
                    using (var reader = new StreamReader(file))
                    {
                        var json = await reader.ReadToEndAsync();
                        var data = JsonConvert.DeserializeObject<ProjectData>(json);
                        var project = ProjectConverter.FromData(data, deserializer, context.Interpreter);
                        project.DefaultSavePath = dialog.FileName;
                        return project;
                    }
                }
            }
            else
            {
                return null;
            }
        }
    }
}
