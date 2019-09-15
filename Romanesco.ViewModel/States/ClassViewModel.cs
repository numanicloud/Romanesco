using System;
using System.Collections.Generic;
using System.Text;
using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Model.States;

namespace Romanesco.ViewModel.States
{
    public class ClassViewModel : IStateViewModel
    {
        private readonly ClassState state;

        public ReactiveProperty<string> Title => state.Title;
        public ReactiveProperty<object> Content => state.Content;
        public ReactiveProperty<string> FormattedString => state.FormattedString;
        public Common.IStateViewModel[] Fields { get; }

        public ClassViewModel(ClassState state, Common.IStateViewModel[] fields)
        {
            this.state = state;
            Fields = fields;
        }
    }
}
