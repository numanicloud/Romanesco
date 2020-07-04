using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
				ProjectTypeQualifier = project.Settings.ProjectType.FullName ?? throw new Exception(),
				ProjectTypeExporterQualifier = project.Settings.ExporterType.FullName ?? throw new Exception(),
				EncodedMaster = serializer.Serialize(project.Root.RootInstance),
				DependencyProjects = project.DependencyProjects.Select(x => x.Path).ToArray()
			};
		}

		public static async Task<Project> FromInstanceAsync(ProjectSettings settings, IStateDeserializer deserializer, ObjectInterpreter interpreter, object instance)
		{
			var properties = settings.ProjectType.GetProperties()
				.Where(p => p.GetCustomAttribute<EditorMemberAttribute>() != null)
				.Select(p => interpreter.InterpretRootAsState(instance, p));
			var fields = settings.ProjectType.GetFields()
				.Where(f => f.GetCustomAttribute<EditorMemberAttribute>() != null)
				.Select(f => interpreter.InterpretRootAsState(instance, f));
			var states = properties.Concat(fields).ToArray();

			// 依存関係を読み込む
			var list = new List<ProjectDependency>();
			foreach (var item in settings.DependencyProjects)
			{
				using (var file = new StreamReader(item))
				{
					try
					{
						var contents = await file.ReadToEndAsync();
						var data = JsonConvert.DeserializeObject<ProjectData>(contents);
						var project = await FromDataAsync(data, deserializer, interpreter);
						list.Add(new ProjectDependency(project, item));
					}
					catch (System.Exception)
					{

						throw;
					}
				}
			}

			var root = new StateRoot(instance, states);

			return new Project(settings, root, list.ToArray());
		}

		public static async Task<Project> FromDataAsync(ProjectData data, IStateDeserializer deserializer, ObjectInterpreter interpreter)
		{
			var assembly = Assembly.LoadFrom(data.AssemblyPath);
			var type = assembly.GetType(data.ProjectTypeQualifier);
			var exporter = assembly.GetType(data.ProjectTypeExporterQualifier) ?? typeof(Services.Export.NewtonsoftJsonProjectTypeExporter);

			if (type == null)
			{
				throw new Exception($"アセンブリからプロジェクト型 {data.ProjectTypeQualifier} を取得できませんでした。");
			}

			var settings = new ProjectSettings(assembly, type, exporter, data.DependencyProjects);
			var instance = deserializer.Deserialize(data.EncodedMaster, type);
			return await FromInstanceAsync(settings, deserializer, interpreter, instance);
		}
	}
}