namespace Romanesco.Model.EditorComponents
{
	internal interface IEditorStateChanger
	{
		void ChangeToEmpty();
		void ChangeToNew();
		void ChangeToClean();
		void ChangeToDirty();
	}
}
