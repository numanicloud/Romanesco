using Microsoft.Extensions.DependencyInjection;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Model.EditorComponents;
using Romanesco.Model.EditorComponents.States;
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
			services.AddSingleton<EmptyEditorState>();
			services.AddSingleton<NewEditorState>();
			services.AddSingleton<CleanEditorState>();
			services.AddSingleton<DirtyEditorState>();
			services.AddSingleton<IProjectLoadService, WindowsLoadService>();
			services.AddSingleton<IProjectSaveService, WindowsSaveService>();
			services.AddSingleton<IProjectHistoryService, SimpleHistoryService>();
			services.AddSingleton<IStateSerializer, NewtonsoftStateSerializer>();
			services.AddSingleton<IStateDeserializer, NewtonsoftStateDeserializer>();
			services.AddTransient<EditorContext>();
			services.AddSingleton<ProjectSettingsEditor>();
			services.AddSingleton<ObjectInterpreter>();
			services.AddSingleton<IObjectInterpreter, ObjectInterpreter>();
		}
	}
}
