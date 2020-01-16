using Livet.Behaviors.Messaging;
using Livet.Messaging;
using Reactive.Bindings;
using Romanesco.Model.EditorComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Romanesco.ViewModel
{
    public class ProjectSettingsEditorViewModel : Livet.ViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ProjectSettingsEditor editor;

        public ReactiveProperty<string> AssemblyPath => editor.AssemblyPath;
        public ReactiveProperty<string> ProjectTypeName => editor.ProjectTypeFullName;
        public ReactiveProperty<string[]> ProjectTypeMenu => editor.ProjectTypeMenu;
        public ReactiveProperty<string> ProjectTypeExporterName => editor.ProjectTypeExporterFullName;
        public ReactiveProperty<string[]> ProjectTypeExporterMenu => editor.ProjectTypeExporterMenu;

        public ProjectSettingsEditorViewModel(ProjectSettingsEditor editor)
        {
            this.editor = editor;
        }

        public void OpenAssembly() => editor.OpenAssembly();

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
