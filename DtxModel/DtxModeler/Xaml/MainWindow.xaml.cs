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
using DtxModeler.Ddl;
using System.Xml.Serialization;
using System.Threading;
using System.Collections.ObjectModel;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private Database current_ddl;
		private TreeViewItem current_ddl_root;

		private List<Database> server_databases = new List<Database>();
		private List<TreeViewItem> server_databases_roots = new List<TreeViewItem>();
		

		private DtxModeler.Ddl.Table selected_table;
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
						current_ddl = (Database)serializer.Deserialize(ddl_stream);
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

			_treDatabaseLayout.Items.Remove(current_ddl_root);

			var db_root = createTreeViewItem(current_ddl.Name, image_database);
			db_root.IsExpanded = true;

			foreach (var table in current_ddl.Table) {
				var tree_table = createTreeViewItem(table.Name, image_table);
				tree_table.Tag = table;
				db_root.Items.Add(tree_table);
			}

			_treDatabaseLayout.Items.Add(db_root);
			current_ddl_root = db_root;
		}

		private void refreshServers() {
			string image_database_connect = "pack://application:,,,/Xaml/Images/database_connect.png";

			foreach (var database_root in server_databases_roots) {
				_treDatabaseLayout.Items.Remove(database_root);
			}

			foreach (var server in current_ddl.Modeler.DbConnection) {
				var server_tree = createTreeViewItem(server.Name, image_database_connect);

			}

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

		private void Open_Click(object sender, RoutedEventArgs e) {
			openDdl();
		}

		private void Save_Click(object sender, RoutedEventArgs e) {
			saveDdl();
		}


		public void saveDdl() {

		}


		private void _treDatabaseLayout_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
			var selected_item = _treDatabaseLayout.SelectedItem as TreeViewItem;
			if ((selected_item != null) && (selected_item.Tag is DtxModeler.Ddl.Table)) {
				selected_table = selected_item.Tag as DtxModeler.Ddl.Table;
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
			_tabColumnProperties.IsEnabled = _grdColumnProperties.IsEnabled = true;

			if (selected_column == null) {
				_txtColumnName.Text = _txtColumnDbType.Text = "";
				_cmbColumnNetType.SelectedIndex = -1;
				_chkColumnNullable.IsChecked = _chkColumnPrimaryKey.IsChecked = false;
				return;
			}

			_txtColumnName.Text = selected_column.Name;
			//_txtColumnType.Text = selected_column.Type;
			_txtColumnDbType.Text = selected_column.DbType;
			_chkColumnNullable.IsChecked = selected_column.Nullable;
			
			// ToDo: Default Value

			_chkColumnPrimaryKey.IsChecked = selected_column.IsPrimaryKey;
			

		}

		private void _btnColumnCancel_Click(object sender, RoutedEventArgs e) {
			selected_column = null;
			refreshCurrentColumn();
			_dagColumnDefinitions.UnselectAll();
			_tabColumnProperties.IsEnabled = _grdColumnProperties.IsEnabled = false;
		}

		private void _btnColumnSave_Click(object sender, RoutedEventArgs e) {
			selected_column.Name = _txtColumnName.Text;
			//selected_column.Type = _txtColumnType.Text;
			selected_column.DbType = _txtColumnDbType.Text;
			selected_column.Nullable = _chkColumnNullable.IsChecked.Value;

			
			
			selected_column.IsPrimaryKey = _chkColumnPrimaryKey.IsChecked.Value;

			_dagColumnDefinitions.Items.Refresh();
		}

		private void AddServer_Click(object sender, RoutedEventArgs e) {

		}

	}
}
