using Deptorygen.Annotations;
using Romanesco.Model.Services.Save;
using Romanesco.Model.States;

namespace Romanesco.Model.Infrastructure;

[Factory]
interface IProjectModelFactory : IModelFactory
{
	IModelFactory Model { get; }

	NewEditorState ResolveNewEditorStateAsTransient();
	CleanEditorState ResolveCleanEditorStateAsTransient();
	DirtyEditorState ResolveDirtyEditorStateAsTransient();

	[Resolution(typeof(WindowsSaveService))]
	IProjectSaveService ResolveSaveService();
}