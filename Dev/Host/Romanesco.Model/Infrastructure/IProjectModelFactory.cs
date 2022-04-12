using Deptorygen.Annotations;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.Infrastructure
{
	[Factory]
	interface IProjectModelFactory : IModelFactory
	{
		IModelFactory Model { get; }

		NewEditorState ResolveNewEditorStateAsTransient();
		CleanEditorState ResolveCleanEditorStateAsTransient();
		DirtyEditorState ResolveDirtyEditorStateAsTransient();
		[Resolution(typeof(WindowsSaveService))]
		IProjectSaveService ResolveProjectSaveService();
	}
}
