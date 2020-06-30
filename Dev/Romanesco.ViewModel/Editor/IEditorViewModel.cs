using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.EditorComponents;

namespace Romanesco.ViewModel.Editor
{
	public interface IEditorViewModel
	{
		ReactiveProperty<IStateViewModel[]> Roots { get; }
		void ShowProjectSetting(ProjectSettingsEditor editor);
	}
}
