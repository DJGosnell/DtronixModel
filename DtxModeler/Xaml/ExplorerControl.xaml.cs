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

		private bool first_rightclick = false;

		private Selection selected_type;

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
		/// Refreshes the control to display the current loaded databases and selects the specified database.
		/// </summary>
		/// <param name="select_db">If a database is provided, it will be automatically selected in the tree.</param>
		private void Refresh() {
			

			string image_database = "pack://application:,,,/Xaml/Images/database.png";
			string image_table = "pack://application:,,,/Xaml/Images/table.png";
			string image_table_custom = "pack://application:,,,/Xaml/Images/table_gear.png";
			string image_tables = "pack://application:,,,/Xaml/Images/table_multiple.png";
			string image_view = "pack://application:,,,/Xaml/Images/table_chart.png";
			string image_function = "pack://application:,,,/Xaml/Images/function.png";

			var last_selected_db = selected_database;
			_TreDatabaseLayout.Items.Clear();
			

			foreach (var database in loaded_databases) {
				var db_root = createTreeViewItem(database.Name, image_database);
				db_root.Tag = database;
				db_root.IsExpanded = true;

				if (last_selected_db == database || _TreDatabaseLayout.SelectedItem == null) {
					db_root.IsSelected = true;
				}


				// Tables
				var tables_root = createTreeViewItem("Tables", image_tables);
				tables_root.Tag = typeof(Table[]);
				tables_root.IsExpanded = true;
				db_root.Items.Add(tables_root);

				// Display all the tables
				foreach (var table in database.Table) {
					var tree_table = createTreeViewItem(table.Name, (table.UseCustomSql)? image_table_custom : image_table);
					tree_table.Tag = table;
					tables_root.Items.Add(tree_table);
					tree_table.IsSelected = (selected_table == table);
				}

				// Views
				var views_root = createTreeViewItem("Views", image_view);
				views_root.Tag = typeof(View[]);
				db_root.Items.Add(views_root);

				// Functions
				var functions_root = createTreeViewItem("Functions", image_function);
				functions_root.Tag = typeof(Function[]);
				db_root.Items.Add(functions_root);

				_TreDatabaseLayout.Items.Add(db_root);
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

			CreateDatabase(dialog.Value);
		}

		/// <summary>
		/// Creates a new database with the specified name.
		/// </summary>
		/// <param name="database_name">Name of the database.</param>
		public void CreateDatabase(string database_name) {
			LoadDatabase(new Database() {
				Name = database_name
			});
		}


		/// <summary>
		/// Prompts the user to select a database and then loads and opens it.
		/// </summary>
		public bool LoadDatabase() {
			var dialog = new OpenFileDialog() {
				Filter = "DDL files (*.ddl)|*.ddl", //Text Files (*.txt)|*.txt|All Files (*.*)|*.*
				Multiselect = true
			};

			var status = dialog.ShowDialog();
			if (status.HasValue == false || status.Value == false) {
				return false;
			}

			Task.Run(() => {
				for (int i = 0; i < dialog.FileNames.Length; i++) {
					LoadDatabase(dialog.FileNames[i]);
				}
			});

			return true;

		}

		/// <summary>
		/// Loads a specified filename as a database.
		/// </summary>
		/// <param name="file_name">Database file to load.</param>
		/// <returns>True on success, false otherwise.</returns>
		public bool LoadDatabase(string file_name) {
			Database db = null;
			try {
				try {
					db = Database.LoadFromFile(file_name);
				} catch (Exception e) {
					// See if we can fix any XML issues.
					RecoverDatabase recover = new RecoverDatabase(file_name);
					db = recover.Recover();

					MessageBox.Show("Error opening database:\r\n" + e.Message + ".\r\n Attempting to recover the file.\r\n\r\nRecovery Log:\r\n" + recover.Log, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					if (db == null) {
						throw e;
					} else {
						db._Modified = true;
					}
				}

				db._FileLocation = file_name;
				Dispatcher.BeginInvoke(new Action(() => {
					LoadDatabase(db);
				}), null);

			} catch (Exception e) {
				MessageBox.Show("Error opening database:\r\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
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

			database.PropertyChanged += property_changed;

			// Tables
			Utilities.BindChangedCollection<Table>(database.Table, collection_changed, (prop_sender, prop_e) => {
				property_changed(prop_sender, prop_e);
				if (prop_e.PropertyName == "UseCustomSql") {
					Refresh();
				}
			});
			Utilities.BindChangedCollection<Association>(database.Association, collection_changed, property_changed);

			foreach (var table in database.Table) {
				Utilities.BindChangedCollection<Column>(table.Column, collection_changed, property_changed);
				Utilities.BindChangedCollection<Index>(table.Index, collection_changed, property_changed);
			}

			// Configurations
			Utilities.BindChangedCollection<Configuration>(database.Configuration, collection_changed, (prop_sender, prop_e) => {
				// Ignore the visibility property because this is internal to the GUI and has nothing to do with the schema.
				if (prop_e.PropertyName != "Visibility") {
					property_changed(prop_sender, prop_e);
				}
			});

			// Functions
			Utilities.BindChangedCollection<Function>(database.Function, collection_changed, property_changed);

			// Views
			Utilities.BindChangedCollection<View>(database.View, collection_changed, property_changed);

			foreach (var view in database.View) {
				Utilities.BindChangedCollection<Column>(view.Column, collection_changed, property_changed);
			}

			loaded_databases.Add(database);
			database.Initialize();
			Refresh();

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
				if (node != null && first_rightclick == false) {
					node.Focus();
					first_rightclick = true;
					first_rightclick = await Task.Run(() => { Thread.Sleep(500); return false; });
				}

			};

			return item;
		}

		public DependencyObject FindVisualParent(DependencyObject obj, Type type) {
			DependencyObject o = obj;
			while (o != null && type != o.GetType()) {
				o = VisualTreeHelper.GetParent(o);
			}
			return o;
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

			_TreDatabaseLayout_SelectedItemChanged(null, null);

			return true;
		}

		private void _TreDatabaseLayout_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
			// Reset out selection.
			selected_database = null;
			selected_table = null;
			selected_function = null;
			selected_view = null;
			selected_type = Selection.None;

			// If nothing is selected, then there is nothing to do.
			if (_TreDatabaseLayout.SelectedItem != null) {

				// Find the root node for the database.
				TreeViewItem root = _TreDatabaseLayout.SelectedItem as TreeViewItem;
				TreeViewItem trasnverse_node = root;

				while (trasnverse_node != null) {
					root = trasnverse_node;
					trasnverse_node = trasnverse_node.Parent as TreeViewItem;
				}

				selected_database = root.Tag as Database;

				// Determine what we have selected.
				var selected_node = _TreDatabaseLayout.SelectedItem as TreeViewItem;
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



		private void _TreDatabaseLayout_DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			switch (selected_type) {
				case Selection.TableItem:
				case Selection.ViewItem:
				case Selection.FunctionItem:
					e.CanExecute = true;
					break;
				default:
					e.CanExecute = false;
					break;
			}

		}

		private void _TreDatabaseLayout_Delete(object sender, ExecutedRoutedEventArgs e) {
			if (selected_type == Selection.TableItem) {
				if (MessageBox.Show("Are you sure you want to delete table \"" + selected_table.Name + "\"?", "Delete Table", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
					selected_database.Table.Remove(selected_table);
				}

			}

			selected_database._Modified = true;
			Refresh();
		}

		private void _TreDatabaseLayout_Paste(object sender, ExecutedRoutedEventArgs e) {
			switch (selected_type) {
				case Selection.Tables:
				case Selection.TableItem:
					var table = Utilities.XmlDeserializeString<Table>(Clipboard.GetText());
					if (table == null) {
						return;
					}

					InputDialogBox.Show("Table Name", "Enter a new name for the table.", table.Name, (value) => {
						table.Name = value;
						selected_database.Table.Add(table);
						Refresh();
					});
					break;
			}
		}

		private void _TreDatabaseLayout_PasteCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			if (Clipboard.ContainsText()) {
				switch (selected_type) {
					case Selection.Tables:
					case Selection.TableItem:
						//case Selection.ViewItem:
						//case Selection.FunctionItem:
						e.CanExecute = true;
						break;
					default:
						e.CanExecute = false;
						break;
				}
			} else {
				e.CanExecute = false;
			}
		}

		private void _TreDatabaseLayout_Copy(object sender, ExecutedRoutedEventArgs e) {
			switch (selected_type) {
				case Selection.TableItem:
					Clipboard.SetText(Utilities.XmlSerializeObject(selected_table), TextDataFormat.UnicodeText);
					break;
				case Selection.ViewItem:
				case Selection.FunctionItem:
					break;
			}
		}

		private void _TreDatabaseLayout_CopyCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			_TreDatabaseLayout_DeleteCanExecute(sender, e);
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

		private void _TreDatabaseLayout_New(object sender, ExecutedRoutedEventArgs e) {

			if (selected_type == Selection.Tables || selected_type == Selection.TableItem) {
				InputDialogBox.Show("Table Name", "Enter a new name for the table.", "", (value) => {
					var table = new Table() {
						Name = value
					};
					selected_database.Table.Add(table);
					Refresh();
				});
			}
		}

		private void _TreDatabaseLayout_NewCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			switch (selected_type) {
				case Selection.Tables:
				case Selection.TableItem:
				//case Selection.Functions:
				//case Selection.FunctionItem:
				//case Selection.Views:
				//case Selection.ViewItem:
					e.CanExecute = true;
					break;
				default:
					e.CanExecute = false;
					break;
			}
		}

		private void _TreDatabaseLayout_Rename(object sender, ExecutedRoutedEventArgs e) {

			if (selected_type == Selection.TableItem) {
				InputDialogBox.Show("Rename Table", "Enter a new name for the table.", selected_table.Name, (value) => {
					selected_table.Rename(selected_database, value);
					Refresh();
				});

			} else if (selected_type == Selection.Database) {
				InputDialogBox.Show("Rename Database", "Enter a new name for the database.", selected_database.Name, (value) => {
					selected_database.Name = value;
					Refresh();
				});
			}
		}

		private void _TreDatabaseLayout_RenameCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			switch (selected_type) {
				case Selection.Database:
				case Selection.TableItem:
				case Selection.ViewItem:
				case Selection.FunctionItem:
					e.CanExecute = true;
					break;
				default:
					e.CanExecute = false;
					break;
			}
		}

		private void _TreDatabaseLayout_OpenDatabaseDirectory(object sender, ExecutedRoutedEventArgs e) {
			if (selected_database._FileLocation != null) {
				Process.Start("explorer.exe", "/select,\"" + selected_database._FileLocation + "\"");
			}
		}

		private void _TreDatabaseLayout_OpenDatabaseDirectoryCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			switch (selected_type) {
				case Selection.Database:
					e.CanExecute = true;
					break;
				default:
					e.CanExecute = false;
					break;
			}
		}

		private void _TreDatabaseLayout_CloseDatabase(object sender, ExecutedRoutedEventArgs e) {
			CloseDatabase(selected_database);
		}

		private void _TreDatabaseLayout_CloseDatabaseCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			_TreDatabaseLayout_OpenDatabaseDirectoryCanExecute(sender, e);
		}

	
	}




}
