using Reactive.Bindings;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.Common.Model.Basics;
using Romanesco.Common.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Implementations;

namespace Romanesco.BuiltinPlugin.ViewModel.States
{
	public class SubtypingClassViewModel : ProxyViewModelBase<SubtypingClassState>
	{
		public ObservableCollection<ISubtypeOption> Choices => State.Choices;
		public ReactiveProperty<ISubtypeOption> SelectedType => State.SelectedType;
		public ReactiveProperty<ClassViewModel?> CurrentViewModel { get; } = new ReactiveProperty<ClassViewModel?>();
		public ReactiveCommand EditCommand { get; }
		public List<IDisposable> Disposables => State.Disposables;

		public SubtypingClassViewModel(SubtypingClassState state, ViewModelInterpretFunc interpreter)
			: base(state)
		{
			void UpdateInstance(IFieldState choice)
			{
				CurrentViewModel.Value = interpreter(choice) as ClassViewModel;
			}

			state.Disposables.Add(CurrentViewModel);

			UpdateInstance(state.CurrentStateReadOnly.Value);
			state.CurrentStateReadOnly.Subscribe(UpdateInstance)
				.AddTo(state.Disposables);

			// 具象型が選択されていなければ詳細表示はできない
			EditCommand = CurrentViewModel.Select(x => x is { })
				.ToReactiveCommand()
				.AddTo(state.Disposables);
			EditCommand.Where(_ => CurrentViewModel.Value is { })
				.Subscribe(_ => ShowDetailSubject.OnNext(Unit.Default))
				.AddTo(state.Disposables);
		}
	}
}
