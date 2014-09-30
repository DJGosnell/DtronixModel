using DtxModeler.Ddl;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for DmlExplorerControl.xaml
	/// </summary>
	public partial class ExplorerControl : UserControl {

		private List<Database> loaded_databases = new List<Database>();

		private ExplorerSelection selected_type;

		private object deserialized_clipboard = null;

		public ExplorerSelection SelectedType {
			get { return selected_type; }
		}


		private Table selected_table;
		private View selected_view;
		private Function selected_function;
		private Database selected_database;

		public Database[] ChangedDatabases {
			get {
				return loaded_databases.Where(db => db._Modified).ToArray();
			}
		}

		public Database SelectedDatabase {
			get { return selected_database; }
		}

		public Table SelectedTable {
			get { return selected_table; }
		}


		public event EventHandler<ExplorerSelectionChangedEventArgs> ChangedSelection;


		public ExplorerControl() {
			selected_type = ExplorerSelection.None;
			this.UseLayoutRounding = true;

			InitializeComponent();
		}

		/// <summary>
		/// Refreshes the control to display the current loaded databases.
		/// </summary>
		private void Refresh() {
			_treDatabaseLayout.Items.Clear();

			string image_database = "pack://application:,,,/Xaml/Images/database.png";
			string image_table = "pack://application:,,,/Xaml/Images/table.png";
			string image_view = "pack://application:,,,/Xaml/Images/table_chart.png";
			string image_function = "pack://application:,,,/Xaml/Images/function.png";

			foreach (var database in loaded_databases) {
				var db_root = createTreeViewItem(database.Name, image_database);
				db_root.Tag = database;
				db_root.IsExpanded = true;

				// Tables
				var tables_root = createTreeViewItem("Tables", image_table);
				tables_root.Tag = typeof(Table[]);
				tables_root.IsExpanded = true;
				db_root.Items.Add(tables_root);

				// Display all the tables
				foreach (var table in database.Table) {
					var tree_table = createTreeViewItem(table.Name, null);
					tree_table.Tag = table;
					tables_root.Items.Add(tree_table);
				}

				// Views
				var views_root = createTreeViewItem("Views", image_view);
				views_root.Tag = typeof(View[]);
				db_root.Items.Add(views_root);

				// Functions
				var functions_root = createTreeViewItem("Functions", image_function);
				functions_root.Tag = typeof(Function[]);
				db_root.Items.Add(functions_root);

				_treDatabaseLayout.Items.Add(db_root);
				database._TreeRoot = db_root;
			}
		}

		public void CreateDdl() {
			loaded_databases.Add(new Database() {
				Table = new Table[0]
			});
			Refresh();
		}

		public void LoadDdl(Database database) {
			
		}

		public void BrowseDdl() {
			Database ddl = null;

			var dialog = new OpenFileDialog() {
				Filter = "Ddl files (*.ddl)|*.ddl",
				Multiselect = true
			};

			var status = dialog.ShowDialog();
			var serializer = new XmlSerializer(typeof(Database));

			if (status.HasValue == false || status.Value == false) {
				return;
			}

			ThreadPool.QueueUserWorkItem(o => {
				Stream[] ddl_streams = dialog.OpenFiles();
				for (int i = 0; i < dialog.FileNames.Length; i++) {
					try {
						ddl = (Database)serializer.Deserialize(ddl_streams[i]);
						ddl._FileLocation = dialog.FileNames[i];
						loaded_databases.Add(ddl);

					} catch (Exception e) {
						ddl_streams[i].Close();
						this.Dispatcher.BeginInvoke(new Action(() => {
							MessageBox.Show("Unable to load selected Ddl file. \r\n" + e.ToString());
						}), null);
						return;
					}
					ddl_streams[i].Close();
				}

				this.Dispatcher.BeginInvoke(new Action(() => {
					Refresh();
				}), null);
			});



		}

		public void Save(Database database, bool save_as) {
			string file_name = null;

			// Save as if specified or force a save as if the default location is not set.
			if (save_as || database._FileLocation == null) {
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
				file_name = database._FileLocation;
			}

			ThreadPool.QueueUserWorkItem(o => {
				using (var stream = File.Open(file_name, FileMode.Create, FileAccess.Write, FileShare.None)) {
					try {
						var serializer = new XmlSerializer(typeof(Database));
						serializer.Serialize(stream, database);
						database.Modeler.Modified = DateTime.Now;
						database._Modified = false;
					} catch (Exception) {
						this.Dispatcher.BeginInvoke(new Action(() => {
							MessageBox.Show("Unable to save current DDL into selected file.");
						}), null);

						return;
					}
				}
			});
		}

		public void SaveCurrent(bool save_as) {
			Save(selected_database, save_as);
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

			TextBlock text = new TextBlock() {
				Text = value,
				TextWrapping = TextWrapping.Wrap,
				Padding = new Thickness(2, 2, 5, 2)
			};

			if (image_path != null) {
				stack.Children.Add(image);
			}

			stack.Children.Add(text);
			item.Header = stack;

			return item;
		}


		private void _treDatabaseLayout_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
			_CmiRename.IsEnabled = _CmiDelete.IsEnabled = _CmiNew.IsEnabled = _CmiPaste.IsEnabled = _CmiCopy.IsEnabled = false;

			switch (selected_type) {
				case ExplorerSelection.Database:
					_CmiRename.IsEnabled = true;
					break;

				case ExplorerSelection.Tables:
				case ExplorerSelection.Views:
				case ExplorerSelection.Functions:
					_CmiPaste.IsEnabled = true;
					break;

				case ExplorerSelection.TableItem:
				case ExplorerSelection.ViewItem:
				case ExplorerSelection.FunctionItem:
					_CmiRename.IsEnabled = _CmiDelete.IsEnabled = _CmiNew.IsEnabled = _CmiCopy.IsEnabled = true;
					break;

				case ExplorerSelection.None:
				default:
					break;
			}

			// See if we have anything in the clipboard to paste.
			if (Clipboard.ContainsText()) {
				string clipboard_text = Clipboard.GetText();

				if ((selected_type == ExplorerSelection.Tables || selected_type == ExplorerSelection.TableItem)
					&& (deserialized_clipboard = Utilities.XmlDeserializeString<Table>(clipboard_text)) != null) {
					_CmiPaste.IsEnabled = true;

				} else if ((selected_type == ExplorerSelection.Views || selected_type == ExplorerSelection.ViewItem)
					&& (deserialized_clipboard = Utilities.XmlDeserializeString<View>(clipboard_text)) != null) {
					_CmiPaste.IsEnabled = true;

				} else if ((selected_type == ExplorerSelection.Functions || selected_type == ExplorerSelection.FunctionItem)
					&& (deserialized_clipboard = Utilities.XmlDeserializeString<Function>(clipboard_text)) != null) {
					_CmiPaste.IsEnabled = true;
				}
			}



		}

		/// <summary>
		/// Closes all the databases open.
		/// </summary>
		/// <returns>True on successful closure of all databases.  False if one of the databases did not close.</returns>
		public bool CloseAllDatabases() {

			foreach (var database in loaded_databases.ToArray()) {
				bool result = CloseDatabase(database);
				if (result == false) {
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Closes the specified database from the explorer.
		/// </summary>
		/// <param name="database">Database to close.</param>
		/// <returns>True on sccessful closure of the datatabase.  False on failure to close.</returns>
		public bool CloseDatabase(Database database) {
			if (database._Modified) {
				var result = MessageBox.Show("Do you want to save changes made to (" + database.Name + ")?", "Save Changes", MessageBoxButton.YesNoCancel);

				switch (result) {
					case MessageBoxResult.None:
					case MessageBoxResult.Cancel:
						return false;

					case MessageBoxResult.No:
						break;

					case MessageBoxResult.Yes:
						Save(database, false);
						break;
				}
				
			}
			loaded_databases.Remove(database);
			Refresh();

			return true;
		}

		private void _CmiClose_Click(object sender, RoutedEventArgs e) {
			if (selected_type == ExplorerSelection.Database) {
				CloseDatabase(selected_database);
			}
		}


		private void _CmiCopy_Click(object sender, RoutedEventArgs e) {
			if (selected_type == ExplorerSelection.TableItem) {
				var text = Utilities.XmlSerializeObject(selected_table);
				Clipboard.SetText(text, TextDataFormat.UnicodeText);
			}
		}

		private void _CmiPaste_Click(object sender, RoutedEventArgs e) {
			if (selected_type == ExplorerSelection.Tables || selected_type == ExplorerSelection.TableItem) {
				Table table = deserialized_clipboard as Table;

				if (table == null) {
					return;
				}

				var dialog = new InputDialogBox() {
					Title = "New Table",
					Description = "Enter a new name for the table.",
					Value = table.Name
				};

				dialog._TxtValue.SelectAll();

				var result = dialog.ShowDialog();

				if (result.HasValue == false || result.Value == false) {
					return;
				}

				table.Name = dialog.Value;

				selected_database._Modified = true;
				Refresh();

			}
		}


		private void _CmiNew_Click(object sender, RoutedEventArgs e) {
			var dialog = new InputDialogBox();
			if (selected_type == ExplorerSelection.Tables || selected_type == ExplorerSelection.TableItem) {
				var table = new Table();

				dialog.Title = "New Table";
				dialog.Description = "Enter a new name for the table.";
				dialog._TxtValue.SelectAll();
				var result = dialog.ShowDialog();

				if (result.HasValue == false || result.Value == false) {
					return;
				}

				table.Name = dialog.Value;
				selected_database.Table = selected_database.Table.Concat(new Table[] { table }).ToArray();
			}

			selected_database._Modified = true;
			Refresh();
		}

		private void _CmiDelete_Click(object sender, RoutedEventArgs e) {
			if (selected_type == ExplorerSelection.Tables || selected_type == ExplorerSelection.TableItem) {
				var result = MessageBox.Show("Are you sure you want to delete table \"" + selected_table.Name + "\"?", "Delete Table", MessageBoxButton.YesNo);

				if (result == MessageBoxResult.Yes) {

					selected_database.Table = selected_database.Table.Where(table => table != selected_table).ToArray();
				}

			}

			selected_database._Modified = true;
			Refresh();
		}


		private void _CmiRename_Click(object sender, RoutedEventArgs e) {
			var dialog = new InputDialogBox();
			if (selected_type == ExplorerSelection.TableItem) {
				dialog.Title = "New Table";
				dialog.Description = "Enter a new name for the table.";
				dialog.Value = selected_table.Name;
				dialog._TxtValue.SelectAll();
				var result = dialog.ShowDialog();

				if (result.HasValue == false || result.Value == false) {
					return;
				}

				selected_table.Name = dialog.Value;
			}

			selected_database._Modified = true;
			Refresh();
		}


		private void _treDatabaseLayout_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {

			// Remove any events and clear the collection to ensure that we do not have events
			// being called after the fact on seperate databases and cause corrupted states.
			/*if (selected_table != null && selected_table._ObservableColumns != null) {
				selected_table._ObservableColumns.Clear();
				selected_table = null;
			}*/

			// Reset out selection.
			selected_database = null;
			selected_table = null;
			selected_function = null;
			selected_view = null;

			// If nothing is selected, then there is nothing to do.
			if (_treDatabaseLayout.SelectedItem != null) {



				// Find the root node for the database.
				TreeViewItem root = _treDatabaseLayout.SelectedItem as TreeViewItem;
				TreeViewItem trasnverse_node = root;

				while (trasnverse_node != null) {
					root = trasnverse_node;
					trasnverse_node = trasnverse_node.Parent as TreeViewItem;
				}

				selected_database = root.Tag as Database;

				// Determine what we have selected.
				var selected_node = _treDatabaseLayout.SelectedItem as TreeViewItem;
				Type node_tag_type = selected_node.Tag as Type;

				if (node_tag_type == null) {
					// We have an item node selected.
					TreeViewItem parent = selected_node.Parent as TreeViewItem;

					if (parent != null) {

						selected_table = selected_node.Tag as Table;
						selected_function = selected_node.Tag as Function;
						selected_view = selected_node.Tag as View;


						// Set the node to the parent so that we can determine what type of node this is.
						node_tag_type = parent.Tag as Type;
					}
				}

				// Narrow down the type of node.
				if (node_tag_type == typeof(Table[])) {
					if (selected_table == null) {
						selected_type = ExplorerSelection.Tables;
					} else {
						selected_type = ExplorerSelection.TableItem;
					}

				} else if (node_tag_type == typeof(Function[])) {
					if (selected_function == null) {
						selected_type = ExplorerSelection.Functions;
					} else {
						selected_type = ExplorerSelection.FunctionItem;
					}

				} else if (node_tag_type == typeof(View[])) {
					if (selected_view == null) {
						selected_type = ExplorerSelection.Views;
					} else {
						selected_type = ExplorerSelection.ViewItem;
					}

				} else if (selected_database != null) {
					selected_type = ExplorerSelection.Database;

				} else {
					selected_type = ExplorerSelection.None;
				}

				if (ChangedSelection != null) {
					ChangedSelection(sender, new ExplorerSelectionChangedEventArgs() {
						SelectionType = selected_type,
						Database = selected_database,
						Table = selected_table,
						Function = selected_function,
						View = selected_view
					});
				}



			}
		}
	
	}

	public class ExplorerSelectionChangedEventArgs : EventArgs {
		public Database Database { get; set; }
		public Table Table { get; set; }
		public Function Function { get; set; }
		public View View { get; set; }
		public ExplorerSelection SelectionType { get; set; }
	}


	public enum ExplorerSelection {
		None,
		Database,
		Tables,
		TableItem,
		Functions,
		FunctionItem,
		Views,
		ViewItem
	}
}
