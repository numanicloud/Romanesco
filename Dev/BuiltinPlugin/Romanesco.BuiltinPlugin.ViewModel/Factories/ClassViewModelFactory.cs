using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings.Extensions;
using Romanesco.BuiltinPlugin.Model.States;
using Romanesco.BuiltinPlugin.ViewModel.States;
using Romanesco.Common.Model.Interfaces;
using Romanesco.Common.ViewModel.Interfaces;

namespace Romanesco.BuiltinPlugin.ViewModel.Factories;

public class ClassViewModelFactory : IStateViewModelFactory
{
	private readonly Dictionary<Type, string> _typeToFocusedTitle = new();

	public IStateViewModel? InterpretAsViewModel(IFieldState state, ViewModelInterpretFunc interpretRecursively)
	{
		if (state is not ClassState @class) return null;

		var fields = @class.Fields.Select(x => interpretRecursively(x)).ToArray();
		var vm = new ClassViewModel(@class, fields);

		foreach (var field in fields)
		{
			field.ShowDetail.Subscribe(_ =>
			{
				vm.CloseUpViewModel.Value = field;
				_typeToFocusedTitle[@class.Type] = field.Title.Value;
			}).AddTo(@class.Disposables);
		}

		vm.OnOpenCommand.Subscribe(_ =>
		{
			if (!_typeToFocusedTitle.TryGetValue(@class.Type, out var title)) return;

			var child = fields.First(x => x.Title.Value == title);
			vm.CloseUpViewModel.Value = child;
			if (child is IOpenCommandConsumer classViewModel)
			{
				classViewModel.OnOpenCommand.Execute();
			}
		}).AddTo(@class.Disposables);

		return vm;
	}
}