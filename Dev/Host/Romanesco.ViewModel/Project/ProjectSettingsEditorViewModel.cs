using System.Collections.ObjectModel;
using Livet.Messaging;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Helpers;
using Romanesco.Model.ProjectComponents;

namespace Romanesco.ViewModel.Project
{
    public class ProjectSettingsEditorViewModel : Livet.ViewModel
    {
        private readonly ProjectSettingsEditor editor;

        public ReactiveProperty<string> AssemblyPath => editor.AssemblyPath;
        public ReactiveProperty<string> ProjectTypeName => editor.ProjectTypeFullName;
        public ReactiveProperty<string[]> ProjectTypeMenu => editor.ProjectTypeMenu;
        public ReactiveProperty<string> ProjectTypeExporterName => editor.ProjectTypeExporterFullName;
        public ReactiveProperty<string[]> ProjectTypeExporterMenu => editor.ProjectTypeExporterMenu;
        public ObservableCollection<string> DependencyProjects => editor.DependencyProjects;
        public ReactiveCommand<object> AddCommand { get; }
        public ReactiveCommand<object> RemoveCommand { get; }

        public ProjectSettingsEditorViewModel(ProjectSettingsEditor editor)
        {
            this.editor = editor;
            AddCommand = new ReactiveCommand<object>();
            RemoveCommand = new ReactiveCommand<object>();

            AddCommand.SubscribeSafe(x => AddDependencyProject())
	            .AddTo(editor.Disposables);
            RemoveCommand.SubscribeSafe(x => RemoveDependencyProject((string)x))
	            .AddTo(editor.Disposables);
        }

        public void OpenAssembly() => editor.OpenAssembly();

        public void AddDependencyProject() => editor.OpenDependencyProject(DependencyProjects.Count);

        public void OpenDependencyProject(string value)
        {
            var index = DependencyProjects.IndexOf(value);
            if (index != -1)
            {
                editor.OpenDependencyProject(index);
            }
        }

        public void RemoveDependencyProject(string value) => editor.RemoveDependencyProject(value);

        public void Confirm()
        {
            editor.Succeeded = true;
            Messenger.Raise(new InteractionMessage("Close"));
        }

        public void Cancel()
        {
            Messenger.Raise(new InteractionMessage("Close"));
        }
    }
}
