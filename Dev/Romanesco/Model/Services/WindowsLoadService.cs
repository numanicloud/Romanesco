using Romanesco.Annotations;
using Romanesco.Common.Entities;
using Romanesco.Model.ProjectComponents;
using System;
using System.Linq;
using System.Reflection;

namespace Romanesco.Model.Services
{
    class WindowsLoadService : IProjectLoadService
    {
        private readonly IProjectSettingProvider projectSettingProvider;

        public WindowsLoadService(IProjectSettingProvider projectSettingProvider)
        {
            this.projectSettingProvider = projectSettingProvider;
        }

        public Project Create(ObjectInterpreter interpreter)
        {
            var settings = projectSettingProvider.GetSettings();
            var properties = settings.ProjectType.GetProperties()
                .Where(p => p.GetCustomAttribute<EditorMemberAttribute>() != null)
                .Select(p => interpreter.InterpretAsState(p));
            var fields = settings.ProjectType.GetFields()
                .Where(f => f.GetCustomAttribute<EditorMemberAttribute>() != null)
                .Select(f => interpreter.InterpretAsState(f));
            var states = properties.Concat(fields).ToArray();
            var root = new StateRoot()
            {
                States = states,
            };

            return new Project(settings, root);
        }

        public Project Open()
        {
            throw new NotImplementedException();
        }
    }
}
