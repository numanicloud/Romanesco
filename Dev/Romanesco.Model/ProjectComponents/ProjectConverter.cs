using System.Linq;
using System.Reflection;
using Romanesco.Annotations;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.Model.ProjectComponents;
using Romanesco.Model.Services.Serialize;

namespace Romanesco.Model.ProjectComponents
{
	public static class ProjectConverter
	{
		public static ProjectData ToData(Project project, IStateSerializer serializer)
		{
			return new ProjectData
			{
                AssemblyPath = project.Settings.Assembly.Location,
                ProjectTypeQualifier = project.Settings.ProjectType.FullName,
                ProjectTypeExporterQualifier = project.Settings.ExporterType.FullName,
                EncodedMaster = serializer.Serialize(project.Root.RootInstance),
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
                States = states,
                RootInstance = instance,
            };

            return new Project(settings, root);
        }

        public static Project FromData(ProjectData data, IStateDeserializer deserializer, ObjectInterpreter interpreter)
        {
            var assembly = Assembly.LoadFrom(data.AssemblyPath);
            var type = assembly.GetType(data.ProjectTypeQualifier);
            var exporter = assembly.GetType(data.ProjectTypeExporterQualifier);

            var settings = new ProjectSettings(assembly, type, exporter);
            var instance = deserializer.Deserialize(data.EncodedMaster, type);
            return FromInstance(settings, interpreter, instance);
        }
	}
}