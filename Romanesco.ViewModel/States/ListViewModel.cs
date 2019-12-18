﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Model.States;

namespace Romanesco.ViewModel.States
{
    public class ListViewModel : IStateViewModel
    {
        private readonly ListState state;
        private readonly ViewModelInterpretFunc interpreter;
        private readonly Subject<Unit> showDetailSubject = new Subject<Unit>();

        public ReactiveProperty<string> Title => state.Title;
        public ReactiveProperty<object> Content => state.Content;
        public ReactiveProperty<IList> ArrayContent => state.ArrayContent;
        public ReactiveProperty<string> FormattedString => state.FormattedString;
        public IObservable<Unit> ShowDetail => showDetailSubject;
        public ReactiveCollection<IStateViewModel> Elements { get; } = new ReactiveCollection<IStateViewModel>();
        public Type ElementType => state.ElementType;
        public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
        public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
        public ReactiveCommand EditCommand { get; } = new ReactiveCommand();
        public IObservable<Exception> OnError => state.OnError;

        public ListViewModel(ListState state, ViewModelInterpretFunc interpreter)
        {
            this.state = state;
            this.interpreter = interpreter;

            EditCommand.Subscribe(x => showDetailSubject.OnNext(Unit.Default));
        }

        public IStateViewModel AddNewElement()
        {
            var element = state.AddNewElement();
            var viewModel = interpreter(element);
            Elements.Add(viewModel);
            return viewModel;
        }

        public void RemoveAt(int index)
        {
            state.RemoveAt(index);
            Elements.RemoveAt(index);
        }
    }
}
