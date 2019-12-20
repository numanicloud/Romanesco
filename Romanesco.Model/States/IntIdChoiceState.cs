using Reactive.Bindings;
using Romanesco.Common;
using Romanesco.Common.Utility;
using Romanesco.Model.Infrastructure;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Romanesco.Model.States
{
    public class IntIdChoiceState : IFieldState
    {
        private readonly Subject<Exception> onErrorSubject = new Subject<Exception>();

        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<object> Content { get; }
        public ReactiveProperty<string> FormattedString { get; } = new ReactiveProperty<string>();
        public Type Type => typeof(int);
        public ValueSettability Settability { get; }
        public IObservable<Exception> OnError => onErrorSubject;
        public ReactiveProperty<MasterList> Master { get; } = new ReactiveProperty<MasterList>();
        public ReactiveProperty<IFieldState> SelectedItem { get; } = new ReactiveProperty<IFieldState>();

        public IntIdChoiceState(ValueSettability settability, string masterName, MasterListContext context)
        {
            Settability = settability;

            // IDisposableをハンドリングするべき。リスト内にこれがある場合、要素削除のときにリークするおそれがある
            context.OnKeyAdded.Where(key => masterName == key)
                .Subscribe(key => UpdateChoices(key, context));

            UpdateChoices(masterName, context);

            Title.Value = settability.MemberName;
            FormattedString = SelectedItem.Select(item =>
            {
                try
                {
                    return item.ToString();
                }
                catch (Exception ex)
                {
                    onErrorSubject.OnNext(ContentAccessException.GetFormattedStringError(ex));
                    return "";
                }
            }).ToReactiveProperty();
            Content = SelectedItem.Select(item => Master.Value.GetId(item.Content.Value))
                .ToReactiveProperty();
        }

        private void UpdateChoices(string masterName, MasterListContext context)
        {
            if (context.Masters.TryGetValue(masterName, out var list) && list.IdType == typeof(int))
            {
                Master.Value = list;
            }
            else
            {
                Master.Value = null;
            }
        }
    }
}