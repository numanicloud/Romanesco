using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Romanesco.Styles.View
{
	/// <summary>
	/// RemovalDistributedList.xaml の相互作用ロジック
	/// </summary>
	public partial class RemovalDistributedList : UserControl
	{
		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register(nameof(ItemsSource),
				typeof(IEnumerable),
				typeof(RemovalDistributedList),
				new FrameworkPropertyMetadata(null, OnItemsSourceChanged));

		public static readonly DependencyProperty ItemTemplateProperty =
			DependencyProperty.Register(nameof(ItemTemplate),
				typeof(DataTemplate),
				typeof(RemovalDistributedList),
				new FrameworkPropertyMetadata(null, OnItemTemplateChanged));

		public static readonly DependencyProperty RemoveCommandProperty =
			DependencyProperty.Register(nameof(RemoveCommand),
				typeof(ICommand),
				typeof(RemovalDistributedList),
				new FrameworkPropertyMetadata(null, OnRemoveCommandChanged));

		public static readonly DependencyProperty AddCommandProperty =
			DependencyProperty.Register(nameof(AddCommand),
				typeof(ICommand),
				typeof(RemovalDistributedList),
				new FrameworkPropertyMetadata(null, OnAddCommandChanged));

		private Panel panel;
		private DataTemplate rowTemplate;

		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		public DataTemplate ItemTemplate
		{
			get => (DataTemplate)GetValue(ItemTemplateProperty);
			set => SetValue(ItemTemplateProperty, value);
		}

		public ICommand RemoveCommand
		{
			get => (ICommand)GetValue(RemoveCommandProperty);
			set => SetValue(RemoveCommandProperty, value);
		}

		public ICommand AddCommand
		{
			get => (ICommand)GetValue(AddCommandProperty);
			set => SetValue(AddCommandProperty, value);
		}

		public RemovalDistributedList()
		{
			InitializeComponent();

			panel = (Panel)Resources["ItemsPanel"];
			rowTemplate = (DataTemplate)Resources["RowTemplate"];
			ListView.Content = panel;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (ItemsSource != null)
			{
				BindItems(ItemsSource);
			}
		}

		private void OnItemsSourceChanged(DependencyPropertyChangedEventArgs args)
		{
			if (args.OldValue != null)
			{
				ClearItems();
			}
			if (args.NewValue != null)
			{
				BindItems((IEnumerable)args.NewValue);
			}
		}

		private void ClearItems()
		{
			panel.Children.Clear();
		}

		private void BindItems(IEnumerable itemsSource)
		{
			foreach (var item in itemsSource)
			{
				if (item is null)
				{
					continue;
				}
				BindItem(item);
			}

			if (itemsSource is INotifyCollectionChanged collectionChanged)
			{
				collectionChanged.CollectionChanged += CollectionChanged_CollectionChanged!;
			}
		}

		private void BindItem(object item)
		{
			if (ItemTemplate.LoadContent() is FrameworkElement obj
				&& rowTemplate.LoadContent() is FrameworkElement wrapper)
			{
				obj.DataContext = item;
				wrapper.DataContext = obj;
				panel.Children.Add(wrapper);
			}
		}

		private void CollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems?.Count > 0)
			{
				for (int i = e.OldItems.Count - 1; i >= 0; i--)
				{
					var index = e.OldStartingIndex + i;
					panel.Children.RemoveAt(index);
				}
			}
			if (e.NewItems?.Count > 0)
			{
				foreach (var item in e.NewItems)
				{
					if (item is null)
					{
						continue;
					}
					BindItem(item);
				}
			}
		}

		private static void OnItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if (obj is RemovalDistributedList control)
			{
				control.OnItemsSourceChanged(args);
			}
		}

		private static void OnItemTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if (obj is RemovalDistributedList control)
			{
			}
		}

		private static void OnRemoveCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
		}

		private static void OnAddCommandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if (obj is RemovalDistributedList control)
			{
				control.AddButton.Command = control.AddCommand;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (((Button)sender).DataContext is FrameworkElement item)
			{
				var dataContext = item.DataContext;
				if (RemoveCommand?.CanExecute(dataContext) == true)
				{
					RemoveCommand.Execute(dataContext);
				}
			}
		}
	}
}
