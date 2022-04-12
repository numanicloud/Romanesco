using Microsoft.Win32;
using Newtonsoft.Json;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.Serialize;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Romanesco.Common.Model.ProjectComponent;
using Romanesco.Common.Model.Reflections;
using Romanesco.Model.Infrastructure;

namespace Romanesco.Model.Services.Load
{
	internal class WindowsLoadService : IProjectLoadService
	{
		private readonly IProjectSettingProvider projectSettingProvider;
		private readonly IStateDeserializer deserializer;
		private readonly IDataAssemblyRepository assemblyRepo;
		private readonly IModelFactory factory;
		private readonly ObjectInterpreter interpreter;

		public WindowsLoadService(IProjectSettingProvider projectSettingProvider,
			IStateDeserializer deserializer,
			IDataAssemblyRepository assemblyRepo,
			IModelFactory factory,
			ObjectInterpreter interpreter)
		{
			this.projectSettingProvider = projectSettingProvider;
			this.deserializer = deserializer;
			this.assemblyRepo = assemblyRepo;
			this.factory = factory;
			this.interpreter = interpreter;
		}

		public bool CanCreate => true;

		public bool CanOpen => true;

		public async Task<IProjectContext?> CreateAsync()
		{
			return await ResetProject(CreateInternalAsync);
		}

		public async Task<IProjectContext?> OpenAsync()
		{
			return await ResetProject(OpenInternalAsync);
		}

		private async Task<ProjectContext?> ResetProject(Func<Task<Project?>> generator)
		{
			var project = await generator();
			if (project != null)
			{
				if (Activator.CreateInstance(project.Settings.ExporterType) is IProjectTypeExporter exporter)
				{
					return new ProjectContext(project, exporter);
				}

				throw new InvalidOperationException($"{project.Settings.ExporterType}をインスタンス化できませんでした。");
			}

			return null;
		}

		private async Task<Project?> CreateInternalAsync()
		{
			using var editor = factory.ResolveProjectSettingsEditorAsTransient();
			var settings = projectSettingProvider.InputCreateSettings(editor);

			if (settings != null)
			{
				if (settings.ProjectType.Assembly.ReflectionOnly)
				{
					var mock = new DynamicMock(
						settings.ProjectType.GetTypeInfo().DeclaredProperties.ToArray(),
						settings.ProjectType.GetTypeInfo().DeclaredFields.ToArray());
					return await ProjectConverter.FromDynamicMockAsync(settings, deserializer, interpreter, mock);
				}

				if (assemblyRepo.CreateInstance(settings.ProjectType) is { } instance)
				{
					return await ProjectConverter.FromInstanceAsync(settings, deserializer, interpreter, instance);
				}

				throw new InvalidOperationException($"プロジェクト型 {settings.ProjectType} のインスタンスを作成できませんでした。");
			}
			return null;
		}

		private async Task<Project?> OpenInternalAsync()
		{
			var dialog = new OpenFileDialog()
			{
				Filter = "マスター プロジェクト (*.roma)|*.roma",
				Title = "マスター プロジェクトを読み込む"
			};

			var result = dialog.ShowDialog();
			if (result == true)
			{
				using var file = File.OpenRead(dialog.FileName);
				using var reader = new StreamReader(file);

				var json = await reader.ReadToEndAsync();
				var data = JsonConvert.DeserializeObject<ProjectData>(json);
				var project = await ProjectConverter.FromDataAsync(data, deserializer, interpreter);
				project.DefaultSavePath = dialog.FileName;
				return project;
			}

			return null;
		}
	}
}
