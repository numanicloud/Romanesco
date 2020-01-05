using Romanesco.Annotations;
using Romanesco.Common.Entities;
using Romanesco.Common.Utility;
using Romanesco.Model.Services.Serialize;
using System.Linq;
using System.Reflection;

namespace Romanesco.Model.ProjectComponents
{
    class Project
    {
        public ProjectSettings Settings { get; }
        public StateRoot Root { get; }
        public string DefaultSavePath { get; set; }

        public Project(ProjectSettings settings, StateRoot root)
        {
            Settings = settings;
            Root = root;
        }

        public ProjectData ToData(IStateSerializer serializer)
        {
            var jsonRoots = Root.States.Select(s => s.Storage.GetValue()).ToArray();
            return new ProjectData
            {
                AssemblyPath = Settings.Assembly.Location,
                ProjectTypeQualifier = Settings.ProjectType.FullName,
                EncodedMaster = serializer.Serialize(jsonRoots),
            };
        }

        public static Project FromInstance(ProjectSettings settings, ObjectInterpreter interpreter, object instance)
        {
            var properties = settings.ProjectType.GetProperties()
                .Where(p => p.GetCustomAttribute<EditorMemberAttribute>() != null)
                .Select(p => interpreter.InterpretRootAsState(instance, p));
            var fields = settings.ProjectType.GetFields()
                .Where(f => f.GetCustomAttribute<EditorMemberAttribute>() != null)
                .Select(f => interpreter.InterpretRootAsState(instance, f));
            var states = properties.Concat(fields).ToArray();

            var root = new StateRoot()
            {
                States = states
            };

            return new Project(settings, root);
        }

        public static Project FromData(ProjectData data, IStateDeserializer deserializer, ObjectInterpreter interpreter)
        {
            var assembly = Assembly.LoadFrom(data.AssemblyPath);
            var type = assembly.GetType(data.ProjectTypeQualifier);
            var settings = new ProjectSettings(assembly, type);
            var instance = deserializer.Deserialize(data.EncodedMaster);
            return FromInstance(settings, interpreter, instance);
        }
    }
}
