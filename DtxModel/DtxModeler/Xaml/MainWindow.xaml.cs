using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Win32;
using DtxModeler.Ddl;
using System.Xml.Serialization;
using System.Threading;
using System.Collections.ObjectModel;
using DtxModeler.Generator.Sqlite;
using DtxModeler.Generator;
using System.IO;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private enum SelectedTreeItemType {
			None,
			Database,
			Tables,
			TableItems,
			Functions,
			FunctionItems,
			Views,
			ViewItems
		}

		private Database current_ddl;
		private string current_ddl_location;
		private TreeViewItem current_ddl_root;
		private bool ddl_changed = false;


		private List<Database> server_databases = new List<Database>();
		private List<TreeViewItem> server_databases_roots = new List<TreeViewItem>();


		private SelectedTreeItemType selected_tree_item_type = SelectedTreeItemType.None;
		private DtxModeler.Ddl.Table selected_table;
		private ObservableCollection<Column> selected_columns;

		private TypeTransformer type_transformer = new SqliteTypeTransformer();
		

		public MainWindow() {
			InitializeComponent();
			_treDatabaseLayout.UseLayoutRounding = true;

			ColumnDbType.ItemsSource = type_transformer.DbTypes();
			ColumnNetType.ItemsSource = type_transformer.NetTypes();
		}

		private Column GetSelectedColumn() {
			return _dagColumnDefinitions.SelectedItem as Column;
			
		}

		private Column[] GetSelectedColumns() {
			return _dagColumnDefinitions.SelectedItems.Cast<Column>().ToArray();
		}

		public void OpenDdl(bool create_blank = false) {
			current_ddl_location = null;
			if (create_blank) {
				this.Title = "Dtronix Modeler - Unsaved Project.ddl";
				current_ddl = new Database(){
					Table = new Table[0]
				};

				refreshDdl();
			} else {
				var dialog = new OpenFileDialog() {
					Filter = "Ddl files (*.ddl)|*.ddl",
					Multiselect = false
				};

				var status = dialog.ShowDialog();
				var serializer = new XmlSerializer(typeof(Database));

				if (status.HasValue == false || status.Value == false) {
					return;
				}

				current_ddl_location = dialog.FileName;
				this.Title = "Dtronix Modeler" + " - " + Path.GetFileName(dialog.FileName);

				ThreadPool.QueueUserWorkItem(o => {
					using (var ddl_stream = dialog.OpenFile()) {
						try {
							current_ddl = (Database)serializer.Deserialize(ddl_stream);
						} catch (Exception e) {
							this.Dispatcher.BeginInvoke(new Action(() => {
								MessageBox.Show("Unable to load selected Ddl file. \r\n" + e.ToString());
							}), null);
							return;
						}
					}

					this.Dispatcher.BeginInvoke(new Action(refreshDdl), null);
				});

			}
			
		}



		public void SaveDdl(bool save_as) {
			string file_name = null;

			// Save as if specified or force a save as if the default location is not set.
			if (save_as || current_ddl_location == null) {
				var dialog = new SaveFileDialog() {
					CheckPathExists = true,
					DefaultExt = "ddl",
					Title = "Save DDL"
				};

				var status = dialog.ShowDialog();

				if (status.HasValue == false || status.Value == false) {
					return;
				}

				file_name = dialog.FileName;
			} else {
				file_name = current_ddl_location;
			}

			current_ddl_location = file_name;
			this.Title = "Dtronix Modeler" + " - " + Path.GetFileName(file_name);

			ThreadPool.QueueUserWorkItem(o => {
				using (var stream = File.Open(file_name, FileMode.Create, FileAccess.Write, FileShare.None)) {
					try {
						var serializer = new XmlSerializer(typeof(Database));
						serializer.Serialize(stream, current_ddl);
						ddl_changed = false;
					} catch (Exception) {
						this.Dispatcher.BeginInvoke(new Action(() => {
							MessageBox.Show("Unable to save current DDL into selected file.");
						}), null);

						return;
					}
				}
			});
		}

		private void refreshDdl() {
			var root = new TreeViewItem();
			root.Header = "Test";
			string image_database = "pack://application:,,,/Xaml/Images/database.png";
			string image_table = "pack://application:,,,/Xaml/Images/table.png";
			string image_view = "pack://application:,,,/Xaml/Images/table_chart.png";
			string image_function = "pack://application:,,,/Xaml/Images/function.png";
			

			_treDatabaseLayout.Items.Remove(current_ddl_root);

			var db_root = createTreeViewItem(current_ddl.Name, image_database);
			db_root.Tag = current_ddl.GetType();
			db_root.IsExpanded = true;

			// Tables
			var tables_root = createTreeViewItem("Tables", image_table);
			tables_root.Tag = typeof(Table[]);
			tables_root.IsExpanded = true;
			db_root.Items.Add(tables_root);

			// Views
			var views_root = createTreeViewItem("Views", image_view);
			views_root.Tag = typeof(View[]);
			db_root.Items.Add(views_root);

			// Functions
			var functions_root = createTreeViewItem("Functions", image_function);
			functions_root.Tag = typeof(Function[]);
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


		private void New_Click(object sender, RoutedEventArgs e) {
			OpenDdl(true);
		}

		private void Open_Click(object sender, RoutedEventArgs e) {
			OpenDdl();
		}

		private void Exit_Click(object sender, RoutedEventArgs e) {
			Exit(new System.ComponentModel.CancelEventArgs());
		}

		private void Save_Click(object sender, RoutedEventArgs e) {
			SaveDdl(false);
		}

		private void SaveAs_Click(object sender, RoutedEventArgs e) {
			SaveDdl(true);
		}


		private void _treDatabaseLayout_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
			ActivateTreeItem();
		}

		void selected_columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			ddl_changed = true;
			selected_table.Column = selected_columns.ToArray();

			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
				foreach (Column column in e.NewItems) {
					column.PropertyChanged += selected_column_PropertyChanged;
				}
			}
		
			
		}


		void selected_column_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			ddl_changed = true;
			Column column = sender as Column;

			// Temporarily remove this event so that we do not get stuck with a stack overflow.
			column.PropertyChanged -= selected_column_PropertyChanged;


			switch (e.PropertyName) {
				case "DbType":
					column.Type = type_transformer.DbToNetType(column.DbType);
					break;

				case "Type":
					column.DbType = type_transformer.NetToDbType(column.Type);
					break;

				case "IsAutoIncrement":
					if (column.IsAutoIncrement) {
						if (new string[] { "system.int16", "system.int32", "system.int64" }.Contains(column.Type.ToLower()) == false) {
							MessageBox.Show("An auto incremented column has to be an integer type.", "Invalid Option");
							column.IsAutoIncrement = false;
						} else if (column.Nullable) {
							MessageBox.Show("Can not auto increment a column that is nullable.", "Invalid Option");
							column.IsAutoIncrement = false;
						}
					}
					break;

				case "Nullable":
					if (column.Nullable && column.IsAutoIncrement) {
						MessageBox.Show("Can not make an auto incremented value nullable.", "Invalid Option");
						column.Nullable = false;
					}
					break;

				case "DefaultValue":
					if (column.IsAutoIncrement) {
						MessageBox.Show("An auto incremented value can not have a default value.", "Invalid Option");
						column.DefaultValue = null;
					}
					break;
			}
					

		


			// Rebind this event to allow us to listen again.
			column.PropertyChanged += selected_column_PropertyChanged;
		}

		private void ActivateTreeItem() {
			selected_table = null;
			
			// Remove all the bound events so that we do not double up on the same change later on.
			if (selected_columns != null) {
				foreach (var column in selected_columns) {
					column.PropertyChanged -= selected_column_PropertyChanged;
				}
			}

			selected_columns = null;
			_dagColumnDefinitions.ItemsSource = null;
			var selected_item = _treDatabaseLayout.SelectedItem as TreeViewItem;

			_TxtTableDescription.IsEnabled = _TxtColumnDescription.IsEnabled = false;
			_TxtTableDescription.Text = _TxtColumnDescription.Text = "";


			if ((selected_item != null) && (selected_item.Tag is DtxModeler.Ddl.Table)) {
				selected_table = selected_item.Tag as DtxModeler.Ddl.Table;

				if (selected_table.Column == null) {
					selected_table.Column = new Column[0];
				}

				selected_columns = new ObservableCollection<Column>(selected_table.Column);
				selected_columns.CollectionChanged += selected_columns_CollectionChanged;

				foreach (var column in selected_columns) {
					column.PropertyChanged += selected_column_PropertyChanged;
				}

				_tabTable.IsSelected = true;
				_dagColumnDefinitions.ItemsSource = selected_columns;

				_TxtTableDescription.IsEnabled = true;
				_TxtTableDescription.Text = selected_table.Description;
				

			}
		}



		private void _dagColumnDefinitions_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
			var column = GetSelectedColumn();
			if (column != null) {

				if (_dagColumnDefinitions.SelectedItems.Count == 1) {
					_TxtColumnDescription.IsEnabled = true;
					_TxtColumnDescription.Text = column.Description;
				}

				_CmiDeleteColumn.IsEnabled = _CmiCopyColumn.IsEnabled = _CmiMoveColumnDown.IsEnabled = _CmiMoveColumnUp.IsEnabled = true;

				// Only allow paste if the clipboard is valid XML.
				if (Clipboard.ContainsText() && Utilities.XmlDeserializeString<DtxModeler.Ddl.Column[]>(Clipboard.GetText()) != null) {
					_CmiPasteColumn.IsEnabled = true;
				}

			} else {
				if (_dagColumnDefinitions.SelectedItems.Count > 1) {

				}
				_TxtColumnDescription.IsEnabled = _CmiDeleteColumn.IsEnabled = _CmiCopyColumn.IsEnabled = _CmiPasteColumn.IsEnabled = _CmiMoveColumnDown.IsEnabled = _CmiMoveColumnUp.IsEnabled = false;
			}
		}

		private void AddServer_Click(object sender, RoutedEventArgs e) {

		}

		private void _treDatabaseLayout_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
			_CmiPasteTable.Visibility = _CmiCopyTable.Visibility = _CmiDeleteTable.Visibility = _CmiCreateTable.Visibility = System.Windows.Visibility.Collapsed;
			var selected_node = _treDatabaseLayout.SelectedItem as TreeViewItem;
			if (selected_node == null) {
				return;
			}

			if (selected_table != null) {
				_CmiCopyTable.Visibility = _CmiDeleteTable.Visibility = _CmiCreateTable.Visibility = System.Windows.Visibility.Visible;

				if (Clipboard.ContainsText() && Utilities.XmlDeserializeString<DtxModeler.Ddl.Table>(Clipboard.GetText()) != null) {
					_CmiPasteTable.Visibility = System.Windows.Visibility.Visible;
				}
			}

			if ((selected_node.Tag as Type) == typeof(Table[])) {
				_CmiPasteTable.Visibility = _CmiCreateTable.Visibility = System.Windows.Visibility.Visible;
			}
			


		}

		private void _treDatabaseLayout_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
			TreeViewItem item = sender as TreeViewItem;
			if (item != null) {
				item.IsSelected = true;
				e.Handled = true;
			}
		}

		private void _TxtColumnDescription_TextChanged(object sender, TextChangedEventArgs e) {
			var column = GetSelectedColumn();
			if (column != null) {
				column.Description = _TxtColumnDescription.Text;
			}
		}

		private void _TxtTableDescription_TextChanged(object sender, TextChangedEventArgs e) {
			if (_TxtTableDescription.IsEnabled) {
				selected_table.Description = _TxtTableDescription.Text;
			}
		}



		private void _CmiDeleteColumn_Click(object sender, RoutedEventArgs e) {
			var columns = GetSelectedColumns();
			string text_columns = "";
			foreach (var column in columns) {
				text_columns += column.Name + "\r\n";
			}

			var result = MessageBox.Show("Are you sure you want to delete the following columns? \r\n" + text_columns, "Confirm Column Deletion", MessageBoxButton.YesNo);

			if (result != MessageBoxResult.Yes) {
				return;
			}

			foreach (var column in columns) {
				selected_columns.Remove(column);
			}

		}

		private void _CmiMoveColumnUp_Click(object sender, RoutedEventArgs e) {
			var columns = GetSelectedColumns();
			foreach (var column in columns) {
				int old_index = selected_columns.IndexOf(column);

				if (old_index <= 0) {
					return;
				}

				selected_columns.Move(old_index, old_index - 1);
			}
		}

		private void _CmiMoveColumnDown_Click(object sender, RoutedEventArgs e) {
			var columns = GetSelectedColumns();
			foreach (var column in columns) {
				int old_index = selected_columns.IndexOf(column);
				int max = selected_columns.Count - 1;

				if (old_index >= max) {
					return;
				}

				selected_columns.Move(old_index, old_index + 1);
			}

		}

		private void _CmiCopyColumn_Click(object sender, RoutedEventArgs e) {
			var text = Utilities.XmlSerializeObject(GetSelectedColumns());
			Clipboard.SetText(text, TextDataFormat.UnicodeText);
		}

		private void _CmiPasteColumn_Click(object sender, RoutedEventArgs e) {
			var columns = Utilities.XmlDeserializeString<DtxModeler.Ddl.Column[]>(Clipboard.GetText());

			if (_MiValidateColumnsOnPaste.IsChecked) {
				foreach (var column in columns) {
					var found_column = selected_columns.FirstOrDefault(col => col.Name.ToLower() == column.Name.ToLower());

					if (found_column != null) {
						var dialog = new InputDialogBox() {
							Title = "Column naming collision",
							Description = "Enter a new name for the old \"" + found_column.Name + "\" Column.",
							Value = found_column.Name
						};

						var result = dialog.ShowDialog();

						if (result.HasValue && result.Value) {
							column.Name = dialog.Value;
						} else {
							return;
						}


					}
				}
			}


			// Add in reverse order to allow for insertion in logical order.
			for (int i = columns.Length - 1; i >= 0; i--) {
				selected_columns.Insert(_dagColumnDefinitions.SelectedIndex + 1, columns[i]);
			}

		}


		private void _CmiCopyTable_Click(object sender, RoutedEventArgs e) {
			var text = Utilities.XmlSerializeObject(selected_table);
			Clipboard.SetText(text, TextDataFormat.UnicodeText);
		}

		private void _CmiPasteTable_Click(object sender, RoutedEventArgs e) {
			if(Clipboard.ContainsText()){
				var table = Utilities.XmlDeserializeString<DtxModeler.Ddl.Table>(Clipboard.GetText());

				var dialog = new InputDialogBox() {
					Title = "New Table",
					Description = "Enter a new name for the table:",
					Value = table.Name
				};

				dialog._TxtValue.SelectAll();

				var result = dialog.ShowDialog();

				if (result.HasValue == false || result.Value == false) {
					return;
				}

				table.Name = dialog.Value;
				current_ddl.Table = current_ddl.Table.Concat(new Table[] { table }).ToArray();

				ddl_changed = true;
				refreshDdl();
			}
		}


		private void _CmiCreateTable_Click(object sender, RoutedEventArgs e) {
			var table = new Table();

			var dialog = new InputDialogBox() {
				Title = "New Table",
				Description = "Enter a new name for the table:"
			};

			dialog._TxtValue.SelectAll();

			var result = dialog.ShowDialog();

			if (result.HasValue == false || result.Value == false) {
				return;
			}

			table.Name = dialog.Value;
			current_ddl.Table = current_ddl.Table.Concat(new Table[] { table }).ToArray();

			ddl_changed = true;
			refreshDdl();
		}

		private void _CmiDeleteTable_Click(object sender, RoutedEventArgs e) {
			var result = MessageBox.Show("Are you sure you want to delete table \"" + selected_table.Name + "\"?", "Delete Table", MessageBoxButton.YesNo);

			if (result == MessageBoxResult.Yes) {
				current_ddl.Table = current_ddl.Table.Where(table => table != selected_table).ToArray();
			}

			ddl_changed = true;
			refreshDdl();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			Exit(e);
		}

		private void Exit(System.ComponentModel.CancelEventArgs e) {
			if (ddl_changed) {
				var result = MessageBox.Show("Changes have been made.  Do you want to save changes?", "Save Changes", MessageBoxButton.YesNoCancel);

				switch (result) {
					case MessageBoxResult.Cancel:
						e.Cancel = true;
						break;
					case MessageBoxResult.None:
					case MessageBoxResult.No:
						break;

					case MessageBoxResult.Yes:
						SaveDdl(false);
						break;
				}
			}
		}


	}

}
