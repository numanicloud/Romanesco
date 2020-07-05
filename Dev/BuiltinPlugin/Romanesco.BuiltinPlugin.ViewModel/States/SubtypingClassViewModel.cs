using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings.Extensions;

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

		public ObservableCollection<ISubtypeOption> Choices => state.Choices;
		public ReactiveProperty<ISubtypeOption> SelectedType => state.SelectedType;
		public ReactiveProperty<ClassViewModel?> CurrentViewModel { get; } = new ReactiveProperty<ClassViewModel?>();
		public ReactiveCommand EditCommand { get; }
		public List<IDisposable> Disposables => state.Disposables;

		public SubtypingClassViewModel(SubtypingClassState state, ViewModelInterpretFunc interpreter)
		{
			this.state = state;

			CurrentViewModel.Value = interpreter(state.CurrentStateReadOnly.Value) as ClassViewModel;
			state.Disposables.Add(CurrentViewModel);

			state.CurrentStateReadOnly.Subscribe(x =>
			{
				CurrentViewModel.Value = interpreter(x) as ClassViewModel;
			}).AddTo(state.Disposables);

			// 具象型が選択されていなければ詳細表示はできない
			EditCommand = CurrentViewModel.Select(x => x is { })
				.ToReactiveCommand();
			EditCommand.Where(_ => CurrentViewModel.Value is { })
				.Subscribe(_ => showDetailSubject.OnNext(Unit.Default))
				.AddTo(state.Disposables);
		}
	}
}
