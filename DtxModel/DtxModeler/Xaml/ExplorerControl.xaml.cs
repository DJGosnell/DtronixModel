using DtxModeler.Ddl;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Xml.Serialization;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for DmlExplorerControl.xaml
	/// </summary>
	public partial class ExplorerControl : UserControl {

		private List<Database> loaded_databases = new List<Database>();

		private Selection selected_type;

		private object deserialized_clipboard = null;

		public Selection SelectedType {
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

		public event EventHandler<DatabaseEventArgs> DatabaseModified;
		public event EventHandler<SelectionChangedEventArgs> ChangedSelection;
		public event EventHandler<DatabaseEventArgs> LoadedDatabase;
		public event EventHandler<DatabaseEventArgs> UnloadedDatabase;


		public ExplorerControl() {
			selected_type = Selection.None;
			this.UseLayoutRounding = true;

			InitializeComponent();
		}

		/// <summary>
		/// Refreshes the control to display the current loaded databases.
		/// </summary>
		private void Refresh() {
			Refresh(null);
		}

		/// <summary>
		/// Refreshes the control to display the current loaded databases and selectes the specified database.
		/// </summary>
		/// <param name="select_db">If a database is provided, it will be automatically selected in the tree.</param>
		private void Refresh(Database select_db) {
			_treDatabaseLayout.Items.Clear();

			string image_database = "pack://application:,,,/Xaml/Images/database.png";
			string image_table = "pack://application:,,,/Xaml/Images/table.png";
			string image_tables = "pack://application:,,,/Xaml/Images/table_multiple.png";
			string image_view = "pack://application:,,,/Xaml/Images/table_chart.png";
			string image_function = "pack://application:,,,/Xaml/Images/function.png";

			foreach (var database in loaded_databases) {
				var db_root = createTreeViewItem(database.Name, image_database);
				db_root.Tag = database;
				db_root.IsExpanded = true;

				if (select_db == database || _treDatabaseLayout.SelectedItem == null) {
					db_root.IsSelected = true;
				}


				// Tables
				var tables_root = createTreeViewItem("Tables", image_tables);
				tables_root.Tag = typeof(Table[]);
				tables_root.IsExpanded = true;
				db_root.Items.Add(tables_root);

				// Display all the tables
				foreach (var table in database.Table) {
					var tree_table = createTreeViewItem(table.Name, image_table);
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

		/// <summary>
		/// Create a new database and prompt the user for a name.
		/// </summary>
		public void CreateDatabase() {
			var dialog = new InputDialogBox() {
				Title = "New Database",
				Description = "Enter a name for the new database."
			};
			var result = dialog.ShowDialog();

			if (result.HasValue == false || result.Value == false) {
				return;
			}

			CreateDAtabase(dialog.Value);
		}

		/// <summary>
		/// Creates a new database with the specified name.
		/// </summary>
		/// <param name="database_name">Name of the database.</param>
		public void CreateDAtabase(string database_name) {
			LoadDatabase(new Database() {
				Name = database_name
			});
		}


		/// <summary>
		/// Promps the user to select a database and then loads and opens it.
		/// </summary>
		public void LoadDatabase() {
			Database database = null;

			var dialog = new OpenFileDialog() {
				Filter = "DDL files (*.ddl)|*.ddl", //Text Files (*.txt)|*.txt|All Files (*.*)|*.*
				Multiselect = true
			};

			var status = dialog.ShowDialog();
			if (status.HasValue == false || status.Value == false) {
				return;
			}

			Task.Run(() => {
				for (int i = 0; i < dialog.FileNames.Length; i++) {
					try {
						database = Database.LoadFromFile(dialog.FileNames[i]);
						database._FileLocation = dialog.FileNames[i];

					} catch (Exception e) {
						this.Dispatcher.Invoke(new Action(() => {
							MessageBox.Show("Unable to load selected Ddl file. \r\n" + e.ToString());
						}), null);
						return;
					}
				}

				this.Dispatcher.BeginInvoke(new Action(() => {
					LoadDatabase(database);
				}), null);
			});



		}

		/// <summary>
		/// Loads the specified database into the control.
		/// </summary>
		/// <param name="database">Database to load.</param>
		public void LoadDatabase(Database database) {
			NotifyCollectionChangedEventHandler collection_changed = (coll_sender, coll_e) => {
				database._Modified = true;
				if (DatabaseModified != null) {
					DatabaseModified(this, new DatabaseEventArgs() {
						Database = database
					});
				}
			};

			PropertyChangedEventHandler property_changed = (prop_sender, prop_e) => {
				database._Modified = true;
				if (DatabaseModified != null) {
					DatabaseModified(this, new DatabaseEventArgs() {
						Database = database
					});
				}
			};

			// Tables
			Utilities.BindChangedCollection<Table>(database.Table, collection_changed, property_changed);
			Utilities.BindChangedCollection<Association>(database.Association, collection_changed, property_changed);

			foreach (var table in database.Table) {
				Utilities.BindChangedCollection<Column>(table.Column, collection_changed, property_changed);
				Utilities.BindChangedCollection<Index>(table.Index, collection_changed, property_changed);
			}

			// Configurations
			Utilities.BindChangedCollection<Configuration>(database.Configuration, collection_changed, property_changed);

			// Functions
			Utilities.BindChangedCollection<Function>(database.Function, collection_changed, property_changed);

			// Views
			Utilities.BindChangedCollection<View>(database.View, collection_changed, property_changed);

			foreach (var view in database.View) {
				Utilities.BindChangedCollection<Column>(view.Column, collection_changed, property_changed);
			}

			loaded_databases.Add(database);
			database.Initialize();
			Refresh(database);

			if (LoadedDatabase != null) {
				LoadedDatabase(this, new DatabaseEventArgs() {
					Database = database
				});
			}

		}

		/// <summary>
		/// Saves the current database into its file.
		/// </summary>
		public void Save() {
			Save(selected_database, false);
		}

		/// <summary>
		/// Saves the current database and optionally prompts the user to save the database as another file.
		/// </summary>
		/// <param name="save_as">True to prompt the user for a new file to save the database into. False to save into the opened file.</param>
		public void Save(bool save_as) {
			Save(selected_database, save_as);
		}

		/// <summary>
		/// Saves the sepecified database and optionally prompts the user to save the database as another file.
		/// </summary>
		/// <param name="database">Database to save.</param>
		/// <param name="save_as">True to prompt the user for a new file to save the database into. False to save into the opened file.</param>
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

			database._FileLocation = file_name;
			database._Modified = false;

			Task.Run(() => {
				Exception exception = null;
				if (database.SaveToFile(file_name, out exception) == false) {
					this.Dispatcher.BeginInvoke(new Action(() => {
						database._Modified = true;
						MessageBox.Show("Unable to save current DDL into selected file. \r\n" + exception.ToString());
					}), null);
				}
			});
		}
		bool isFirstTime = false;

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

			

			item.MouseDown += async(object sender, MouseButtonEventArgs e)  => {


				var node = sender as TreeViewItem;
				if (node != null && isFirstTime == false) {
					node.Focus();
					isFirstTime = true;
					isFirstTime = await Task.Run(() => { Thread.Sleep(500); return false; });
				}


				/*if (FindVisualParent((DependencyObject)e.OriginalSource, e.Source.GetType()) == sender) {
					var selected = _treDatabaseLayout.SelectedItem as TreeViewItem;
					if (selected.IsSelected) {
						selected.IsSelected = false;
					}
					VisualTreeHelper.

					item.IsSelected = true;
				}
				*/
				//if (e.RightButton == MouseButtonState.Pressed) {
					
				//}
			};

			return item;
		}

		/*private async void ResetRightClickAsync() {
			isFirstTime = await SetFirstTimeToFalse();
		}

		private async Task<bool> SetFirstTimeToFalse() {
			return ;
		}*/

		public DependencyObject FindVisualParent(DependencyObject obj, Type type) {
			DependencyObject o = obj;
			while (o != null && type != o.GetType()) {
				o = VisualTreeHelper.GetParent(o);
			}
			return o;
		}

		private void _treDatabaseLayout_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
			_CmiRename.IsEnabled = _CmiDelete.IsEnabled = _CmiNew.IsEnabled = _CmiPaste.IsEnabled = _CmiCopy.IsEnabled = false;
			_CmiClose.Visibility = _CmiBrowse.Visibility = System.Windows.Visibility.Collapsed;

			switch (selected_type) {
				case Selection.Database:
					_CmiRename.IsEnabled = true;
					_CmiClose.Visibility = _CmiBrowse.Visibility = System.Windows.Visibility.Visible;
					break;

				case Selection.Tables:
				case Selection.Views:
				case Selection.Functions:

					_CmiNew.IsEnabled = _CmiPaste.IsEnabled = true;
					break;

				case Selection.TableItem:
				case Selection.ViewItem:
				case Selection.FunctionItem:
					_CmiRename.IsEnabled = _CmiDelete.IsEnabled = _CmiNew.IsEnabled = _CmiCopy.IsEnabled = true;
					break;

				case Selection.None:
				default:
					break;
			}

			// See if we have anything in the clipboard to paste.
			if (Clipboard.ContainsText()) {
				string clipboard_text = Clipboard.GetText();

				if ((selected_type == Selection.Tables || selected_type == Selection.TableItem)
					&& (deserialized_clipboard = Utilities.XmlDeserializeString<Table>(clipboard_text)) != null) {
					_CmiPaste.IsEnabled = true;

				} else if ((selected_type == Selection.Views || selected_type == Selection.ViewItem)
					&& (deserialized_clipboard = Utilities.XmlDeserializeString<View>(clipboard_text)) != null) {
					_CmiPaste.IsEnabled = true;

				} else if ((selected_type == Selection.Functions || selected_type == Selection.FunctionItem)
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

			if (UnloadedDatabase != null) {
				UnloadedDatabase(this, new DatabaseEventArgs() {
					Database = database
				});
			}

			loaded_databases.Remove(database);
			Refresh();

			_treDatabaseLayout_SelectedItemChanged(null, null);

			return true;
		}


		private void _CmiBrowse_Click(object sender, RoutedEventArgs e) {
			if (selected_database._FileLocation != null) {
				Process.Start("explorer.exe", "/select,\"" + selected_database._FileLocation + "\"");
			}
		}

		private void _CmiClose_Click(object sender, RoutedEventArgs e) {
			if (selected_type == Selection.Database) {
				CloseDatabase(selected_database);
			}
		}


		private void _CmiCopy_Click(object sender, RoutedEventArgs e) {
			if (selected_type == Selection.TableItem) {
				var text = Utilities.XmlSerializeObject(selected_table);
				Clipboard.SetText(text, TextDataFormat.UnicodeText);
			}
		}

		private void _CmiPaste_Click(object sender, RoutedEventArgs e) {
			if (selected_type == Selection.Tables || selected_type == Selection.TableItem) {
				Table table = deserialized_clipboard as Table;

				if (table == null) {
					return;
				}

				Action database_changed = () => {
					selected_database._Modified = true;
					Refresh();
				};

				if (selected_type == Selection.TableItem) {
					InputDialogBox.Show("Table Name", "Enter a new name for the table.", table.Name, (value) => {
						table.Name = value;
						selected_database.Table.Add(table);
						database_changed();
					});

				}
			}
		}


		private void _CmiNew_Click(object sender, RoutedEventArgs e) {
			Action database_changed = () => {
				selected_database._Modified = true;
				Refresh();
			};

			if (selected_type == Selection.Tables || selected_type == Selection.TableItem) {
				var table = new Table();

				InputDialogBox.Show("Table Name", "Enter a new name for the table.", table.Name, (value) => {
					table.Name = value;
					selected_database.Table.Add(table);
					database_changed();
				});

				
			}
		}

		private void _CmiDelete_Click(object sender, RoutedEventArgs e) {
			if (selected_type == Selection.Tables || selected_type == Selection.TableItem) {
				if (MessageBox.Show("Are you sure you want to delete table \"" + selected_table.Name + "\"?", "Delete Table", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
					selected_database.Table.Remove(selected_table);
				}

			}

			selected_database._Modified = true;
			Refresh();
		}


		private void _CmiRename_Click(object sender, RoutedEventArgs e) {
			Action database_changed = () => {
				selected_database._Modified = true;
				Refresh();
			};

			if (selected_type == Selection.TableItem) {
				InputDialogBox.Show("Rename Table", "Enter a new name for the table.", selected_table.Name, (value) => {
					selected_table.Name = value;
					database_changed();
				});

			} else if (selected_type == Selection.Database) {
				InputDialogBox.Show("Rename Database", "Enter a new name for the database.", selected_database.Name, (value) => {
					selected_database.Name = value;
					database_changed();
				});
			}
		}


		private void _treDatabaseLayout_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
			// Reset out selection.
			selected_database = null;
			selected_table = null;
			selected_function = null;
			selected_view = null;
			selected_type = Selection.None;

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
						selected_type = Selection.Tables;
					} else {
						selected_type = Selection.TableItem;
					}

				} else if (node_tag_type == typeof(Function[])) {
					if (selected_function == null) {
						selected_type = Selection.Functions;
					} else {
						selected_type = Selection.FunctionItem;
					}

				} else if (node_tag_type == typeof(View[])) {
					if (selected_view == null) {
						selected_type = Selection.Views;
					} else {
						selected_type = Selection.ViewItem;
					}

				} else if (selected_database != null) {
					selected_type = Selection.Database;

				}
			}


			if (ChangedSelection != null) {
				ChangedSelection(sender, new SelectionChangedEventArgs() {
					SelectionType = selected_type,
					Database = selected_database,
					Table = selected_table,
					Function = selected_function,
					View = selected_view
				});
			}
		}


		public class SelectionChangedEventArgs : EventArgs {
			public Database Database { get; set; }
			public Table Table { get; set; }
			public Function Function { get; set; }
			public View View { get; set; }
			public Selection SelectionType { get; set; }
		}


		public class DatabaseEventArgs : EventArgs {
			public Database Database { get; set; }
		}

		public enum Selection {
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




}
