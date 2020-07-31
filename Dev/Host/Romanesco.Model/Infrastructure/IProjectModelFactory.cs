using Deptorygen.Annotations;
using Romanesco.Model.EditorComponents.States;
using Romanesco.Model.Services.Save;

namespace Romanesco.Model.Infrastructure
{
	//TODO: 解決メソッドの依存先のうち、キャプチャから解決できるものがコンストラクタ引数で要求されてしまっている
	//TODO: 自分自身が戻り値の解決メソッドを、thisではなくキャプチャから解決してしまう

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
