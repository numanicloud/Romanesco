using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.ProjectComponents;
using Romanesco.Model.Services.History;
using Romanesco.Model.Services.Load;
using Romanesco.Model.Services.Save;
using Romanesco.Model.Services.Serialize;

namespace Romanesco.Model
{
	public class ModelServiceStartUp
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IEditorFacade, Editor>();
			services.AddSingleton<EditorStateChanger>();
			services.AddSingleton<EmptyEditorState>();
			services.AddSingleton<NewEditorState>();
			services.AddSingleton<CleanEditorState>();
			services.AddSingleton<DirtyEditorState>();
			services.AddSingleton<IStateSerializer, NewtonsoftStateSerializer>();
			services.AddSingleton<IStateDeserializer, NewtonsoftStateDeserializer>();
			services.AddSingleton<IProjectLoadService, WindowsLoadService>();
			services.AddSingleton<IProjectHistoryService, SimpleHistoryService>();
			services.AddTransient<ProjectSettingsEditor>();
			services.AddSingleton<ObjectInterpreter>();
			services.AddSingleton<IObjectInterpreter, ObjectInterpreter>();

			/*	ServiceProvider だけで依存解決できないオブジェクトのうち、
				インターフェースとして注入したいものはファクトリを挟む		*/
			services.AddSingleton<ProjectSaveServiceFactory>();
		}
	}
}
