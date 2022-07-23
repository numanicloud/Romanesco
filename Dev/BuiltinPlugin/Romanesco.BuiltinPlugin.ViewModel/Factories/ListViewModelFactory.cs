using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.Model.Exceptions;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.Factories;

public class ListViewModelFactory : IStateViewModelFactory
{
	public IStateViewModel? InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
	{
		if (state is not ListState array) return null;

		var onError = new Subject<Exception>();

		var vm = new ListViewModel(array, interpretRecursively);
		vm.SelectedIndex.Where(i => i < vm.Elements.Count)
			.Subscribe(index => OnSelectedIndexChanged(index, vm, onError))
			.AddTo(array.Disposables);

		vm.OnError = vm.OnError.Merge(onError);

		return vm;
	}

	private void OnSelectedIndexChanged
		(int index, ListViewModel array, Subject<Exception> onError)
	{
		try
		{
			if (index >= 0)
			{
				var element = array.Elements[index];
				array.ClosedUpViewModel.Value = element;
				if (element is IOpenCommandConsumer classViewModel)
				{
					classViewModel.OnOpenCommand.Execute();
				}
			}
			else
			{
				array.ClosedUpViewModel.Value = null;
			}
		}
		catch (Exception ex)
		{
			onError.OnNext(ContentAccessException.GetListError(ex));
		}
	}
}