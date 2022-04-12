using System;
using System.Collections.Generic;
using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.ViewModel.Editor
{
	public interface IEditorViewModel
	{
		List<IDisposable> Disposables { get; }
		ReactiveProperty<IStateViewModel[]> Roots { get; }
		void ShowProjectSetting(ProjectSettingsEditor editor);
	}
}
