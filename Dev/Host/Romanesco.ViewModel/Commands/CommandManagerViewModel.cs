﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Romanesco.Common.Model.Helpers;
using Romanesco.Common.ViewModel.Interfaces;
using Romanesco.Model.Interfaces;
using Romanesco.ViewModel.States;

namespace Romanesco.ViewModel.Commands
{
	internal class CommandManagerViewModel : IDisposable
	{
		private readonly ICommandAvailabilityPublisher model;
		private readonly List<IDisposable> disposables = new List<IDisposable>();
		private readonly IViewModelInterpreter interpreter;
		
		public ReactiveProperty<IStateViewModel[]> Roots { get; }
		public BooleanUsingScopeSource CommandExecution { get; } = new BooleanUsingScopeSource();
		public ReactiveCommand Create { get; }
		public ReactiveCommand Open { get; }
		public ReactiveCommand Save { get; }
		public ReactiveCommand SaveAs { get; }

		public CommandManagerViewModel(ICommandAvailabilityPublisher model,
			ReactiveProperty<IStateViewModel[]> roots, IViewModelInterpreter interpreter)
		{
			Create = ToEditorCommand(model.CanCreate);
			Open = ToEditorCommand(model.CanOpen);
			Save = ToEditorCommand(model.CanSave);
			SaveAs = ToEditorCommand(model.CanSaveAs);
			
			Create.SubscribeSafe(x => CreateAsync().Forget())
				.AddTo(disposables);

			Open.SubscribeSafe(x => OpenAsync().Forget())
				.AddTo(disposables);
			
			Save.SubscribeSafe(x => SaveAsync().Forget())
				.AddTo(disposables);

			SaveAs.SubscribeSafe(x => SaveAsAsync().Wait())
				.AddTo(disposables);

			this.model = model;
			Roots = roots;
			this.interpreter = interpreter;
		}
		
		private ReactiveCommand ToEditorCommand(IObservable<bool> stream)
		{
			var canExecute = stream
				.Concat(CommandExecution.IsUsing.Select(x => !x));
			var scheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);
			return new ReactiveCommand(canExecute, scheduler);
		}
		
		private async Task CreateAsync()
		{
			// 実行をロックする機能は IProjectLoadService 側に入れるべきかも
			using (CommandExecution.Create())
			{
				var projectContext = await model.CreateAsync();
				if (projectContext != null)
				{
					Roots.Value = projectContext.Project.Root.States
						.Select(s => interpreter.InterpretAsViewModel(s))
						.ToArray();
				}
			}
		}
		
		private async Task OpenAsync()
		{
			using (CommandExecution.Create())
			{
				var projectContext = await model.OpenAsync();
				if (projectContext == null)
				{
					return;
				}

				Roots.Value = projectContext.Project.Root.States
					.Select(s => interpreter.InterpretAsViewModel(s))
					.ToArray();
			}
		}

		private async Task SaveAsync()
		{
			using (CommandExecution.Create())
			{
				await model.SaveAsync();
			}
		}

		private async Task SaveAsAsync()
		{
			using (CommandExecution.Create())
			{
				await model.SaveAsAsync();
			}
		}

		public void Dispose()
		{
			disposables.ForEach(x => x.Dispose());
		}
	}
}
