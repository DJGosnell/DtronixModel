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
using DtxModeler.Generator.Sqlite;
using DtxModeler.Generator;

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

		private TypeTransformer type_transformer = new SqliteTypeTransformer();
		

		public MainWindow() {
			InitializeComponent();
			_treDatabaseLayout.UseLayoutRounding = true;


			ColumnDbType.ItemsSource = type_transformer.DbTypes();
			ColumnNetType.ItemsSource = type_transformer.NetTypes();
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
			string image_view = "pack://application:,,,/Xaml/Images/table_chart.png";
			string image_enum = "pack://application:,,,/Xaml/Images/check_box_list.png";
			string image_function = "pack://application:,,,/Xaml/Images/function.png";
			

			_treDatabaseLayout.Items.Remove(current_ddl_root);

			var db_root = createTreeViewItem(current_ddl.Name, image_database);
			db_root.IsExpanded = true;

			// Tables
			var tables_root = createTreeViewItem("Tables", image_table);
			tables_root.IsExpanded = true;
			db_root.Items.Add(tables_root);

			// Views
			var views_root = createTreeViewItem("Views", image_view);
			db_root.Items.Add(views_root);

			// Functions
			var functions_root = createTreeViewItem("Functions", image_function);
			db_root.Items.Add(functions_root);

			foreach (var table in current_ddl.Table) {
				var tree_table = createTreeViewItem(table.Name, null);
				tree_table.Tag = table;
				tables_root.Items.Add(tree_table);
			}

			_treDatabaseLayout.Items.Add(db_root);
			current_ddl_root = db_root;
		}

		private TreeViewItem createTreeViewItem(string value, string image_path) {
			TreeViewItem item = new TreeViewItem();
			Image image = null;

			StackPanel stack = new StackPanel();
			stack.Orientation = Orientation.Horizontal;

			// create Image
			if (image_path != null) {
				image = new Image();
				image.Source = new BitmapImage(new Uri(image_path));
			}
			//RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
			//RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);
			

			TextBlock text = new TextBlock();
			text.Text = value;
			text.TextWrapping = TextWrapping.Wrap;
			text.Padding = new Thickness(2, 2, 5, 2);
			if (image_path != null) {
				stack.Children.Add(image);
			}
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


			var column = _dagColumnDefinitions.SelectedItem as Column;

			if (column != null) {
				selected_column = column;
			}

		}

		private void AddServer_Click(object sender, RoutedEventArgs e) {

		}

		private void _dagColumnDefinitions_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
			if (e.EditAction == DataGridEditAction.Cancel || selected_column == null) {
				return;
			}
			
			if (e.Column == ColumnDbType) {
				selected_column.Type = type_transformer.DbToNetType((e.EditingElement as ComboBox).Text);
			}

			if (e.Column == ColumnNetType) {
				selected_column.DbType = type_transformer.NetToDbType((e.EditingElement as ComboBox).Text);
			}
		}

		private void _treDatabaseLayout_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
			if (selected_table == null) {
				e.
			}
		}

	}
}
