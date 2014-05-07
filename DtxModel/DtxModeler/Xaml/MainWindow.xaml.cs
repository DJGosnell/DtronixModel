using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using DtxModel.Ddl;
using System.Xml.Serialization;
using System.Threading;
using System.Collections.ObjectModel;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private Database ddl;

		private DtxModel.Ddl.Table selected_table;
		private Column selected_column;

		public MainWindow() {
			InitializeComponent();
			_treDatabaseLayout.UseLayoutRounding = true;
		}

		public void openDdl() {
			var dialog = new OpenFileDialog() {
				Filter = "Ddl files (*.ddl)|*.ddl",
				Multiselect = false
			};

			var status = dialog.ShowDialog();
			var serializer = new XmlSerializer(typeof(Database));

			if (status.HasValue == false || status.Value == false) {
				return;
			}

			ThreadPool.QueueUserWorkItem(o => { 
				using (var ddl_stream = dialog.OpenFile()) {
					try {
						ddl = (Database)serializer.Deserialize(ddl_stream);
					} catch (Exception) {
						this.Dispatcher.BeginInvoke(new Action(() => {
							MessageBox.Show("Unable to load selected Ddl file.");
						}), null);
						return;
					}
				}

				this.Dispatcher.BeginInvoke(new Action(refreshDdl), null);
			});

			
		}

		private void refreshDdl() {
			var root = new TreeViewItem();
			root.Header = "Test";
			string image_database = "pack://application:,,,/Xaml/Images/database.png";
			string image_table = "pack://application:,,,/Xaml/Images/table.png";
			string image_view = "pack://application:,,,/Xaml/Images/table.png";
			string image_function = "pack://application:,,,/Xaml/Images/plugin.png";


			_treDatabaseLayout.Items.Clear();

			var db_root = createTreeViewItem(ddl.Name, image_database);
			db_root.IsExpanded = true;

			foreach (var table in ddl.Table) {
				var tree_table = createTreeViewItem(table.Name, image_table);
				tree_table.Tag = table;
				db_root.Items.Add(tree_table);
			}

			_treDatabaseLayout.Items.Add(db_root);


			//_treDatabaseLayout.Items.Add()


			/*_treDatabaseLayout.DataContext = new {
				Tables = new ObservableCollection<DtxModel.Ddl.Table>(ddl.Table)
			};*/
		}

		private TreeViewItem createTreeViewItem(string value, string image_path) {
			TreeViewItem item = new TreeViewItem();
			

			StackPanel stack = new StackPanel();
			stack.Orientation = Orientation.Horizontal;

			// create Image
			Image image = new Image();
			image.Source = new BitmapImage(new Uri(image_path));
			//RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
			//RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);
			

			TextBlock text = new TextBlock();
			text.Text = value;
			text.TextWrapping = TextWrapping.Wrap;
			text.Padding = new Thickness(2, 2, 5, 2);

			stack.Children.Add(image);
			stack.Children.Add(text);

			item.Header = stack;

			return item;
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e) {
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			//e.
		}

		private void _miOpen_Click(object sender, RoutedEventArgs e) {
			openDdl();
		}

		private void _treDatabaseLayout_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
			var selected_item = _treDatabaseLayout.SelectedItem as TreeViewItem;
			if ((selected_item != null) && (selected_item.Tag is DtxModel.Ddl.Table)) {
				selected_table = selected_item.Tag as DtxModel.Ddl.Table;
			}

			refreshCurrentTable();
		}

		private void refreshCurrentTable() {
			if (selected_table == null) {
				return;
			}

			_tabTable.IsSelected = true;
			_dagColumnDefinitions.ItemsSource = selected_table.Column;
		}

		private void _dagColumnDefinitions_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (e.AddedItems.Count == 0) {
				return;
			}

			var column = e.AddedItems[0] as Column;

			if (column != null) {
				selected_column = column;
				refreshCurrentColumn();
			}
		}

		private void refreshCurrentColumn() {
			if (selected_column == null) {
				return;
			}

			_txtColumnType.Text = selected_column.Type;
			_cmbColumnNullable.SelectedIndex = (selected_column.CanBeNull) ? 1 : 0;
			_txtColumnName.Text = selected_column.Name;
			_txtColumnDefaultValue.Text = "";

		}

	}
}
