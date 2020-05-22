using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.ViewModel.States;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Romanesco.BuiltinPlugin.ViewModel.States
{
	public class SubtypingClassViewModel : IStateViewModel
	{
		private readonly SubtypingClassState state;
		private readonly Subject<Unit> showDetailSubject = new Subject<Unit>();

		public IReadOnlyReactiveProperty<string> Title => state.Title;
		public IReadOnlyReactiveProperty<string> FormattedString => state.FormattedString;
		public IObservable<Unit> ShowDetail => showDetailSubject;
		public IObservable<Exception> OnError => state.OnError;

		public ObservableCollection<Type> Choices => state.Choices;
		public ReactiveProperty<Type?> SelectedType => state.SelectedType;
		public ReactiveProperty<ClassViewModel?> CurrentViewModel { get; } = new ReactiveProperty<ClassViewModel?>();
		public ReactiveCommand EditCommand { get; }

		public SubtypingClassViewModel(SubtypingClassState state, ViewModelInterpretFunc interpreter)
		{
			this.state = state;

			CurrentViewModel.Value = interpreter(state.CurrentStateReadOnly.Value) as ClassViewModel;

			state.CurrentStateReadOnly.Subscribe(x =>
			{
				CurrentViewModel.Value = interpreter(x) as ClassViewModel;
			});

			// 具象型が選択されていなければ詳細表示はできない
			EditCommand = CurrentViewModel.Select(x => x is { })
				.ToReactiveCommand();
			EditCommand.Where(_ => CurrentViewModel.Value is { })
				.Subscribe(_ => showDetailSubject.OnNext(Unit.Default));
		}
	}
}
