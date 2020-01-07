﻿using Reactive.Bindings;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Romanesco.Common.Model;
using Romanesco.Model.EditorComponents;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel
{
    public class EditorViewModel
    {
        public IEditorFacade Editor { get; set; }
        public ReactiveProperty<IStateViewModel[]> Roots { get; } = new ReactiveProperty<IStateViewModel[]>();

        public ReactiveCommand CreateCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand ExportCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand OpenCommand { get; } = new ReactiveCommand();
        public ReactiveCommand SaveCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand SaveAsCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand Undo { get; } = new ReactiveCommand();
        public ReactiveCommand Redo { get; } = new ReactiveCommand();
        public ReactiveCommand GcDebugCommand { get; } = new ReactiveCommand();

        public EditorViewModel(IEditorFacade editor, IStateViewModelFactoryProvider factoryProvider)
        {
            Editor = editor;

            CreateCommand.SubscribeSafe(x =>
            {
                var interpreter = CreateInterpreter(factoryProvider);
                var projectContext = Editor.Create();
                Roots.Value = projectContext.Project.Root.States
                    .Select(s => interpreter.InterpretAsViewModel(s))
                    .ToArray();
            });

            OpenCommand.SubscribeSafe(x => Open(factoryProvider));

            ExportCommand.SubscribeSafe(x => Editor.Export());
            SaveCommand.SubscribeSafe(x => Editor.SaveAsync().Wait());
            SaveAsCommand.SubscribeSafe(x => Editor.SaveAsAsync().Wait());

            Undo.SubscribeSafe(x => Editor.Undo());
            Redo.SubscribeSafe(x => Editor.Redo());

            GcDebugCommand.SubscribeSafe(x => GC.Collect());
        }

        private ViewModelInterpreter CreateInterpreter(IStateViewModelFactoryProvider factoryProvider)
        {
            return new ViewModelInterpreter(factoryProvider.GetStateViewModelFactories().ToArray());
        }

        private async Task Open(IStateViewModelFactoryProvider factoryProvider)
        {
            var interpreter = CreateInterpreter(factoryProvider);
            var projectContext = await Editor.OpenAsync();
            Roots.Value = projectContext.Project.Root.States
                .Select(s => interpreter.InterpretAsViewModel(s))
                .ToArray();
        }
    }
}