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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Documents;
using System.Diagnostics;
using DtxModeler.Generator.MySqlMwb;
using DtxModeler.Generator.MySql;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private TypeTransformer type_transformer;

		public MainWindow() {
			InitializeComponent();
			ColumnNetType.ItemsSource = Enum.GetValues(typeof(NetTypes)).Cast<NetTypes>();
			_CmbTargetDatabase.ItemsSource = Enum.GetValues(typeof(DbProvider)).Cast<DbProvider>();

			BindCommand(Commands.NewDatabase, null, Command_NewDatabase);
			BindCommand(ApplicationCommands.Open, new KeyGesture(Key.O, ModifierKeys.Control), Command_Open);
			BindCommand(Commands.ImportDatabase, new KeyGesture(Key.I, ModifierKeys.Control), Command_Import);
			BindCommand(Commands.ImportMySqlMwb, null, Command_ImportMySqlMwb);
			BindCommand(ApplicationCommands.Save, new KeyGesture(Key.S, ModifierKeys.Control), Command_Save, Command_SaveCanExecute);
			BindCommand(ApplicationCommands.SaveAs, new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Alt), Command_SaveAs, Command_SaveCanExecute);
			BindCommand(Commands.Exit, new KeyGesture(Key.F4, ModifierKeys.Alt), Command_Exit);
			BindCommand(Commands.GenerateAll, null, Command_GenerateAll, Command_GenerateAllCanExecute);

			_Status.SetStatus("Application Loaded And Ready", ColorStatusBar.Status.Completed);
		}

		private async void Command_ImportMySqlMwb(object sender, ExecutedRoutedEventArgs e) {
			_Status.SetStatus("Importing MySQL Workbench model.", ColorStatusBar.Status.Working);
			var browse = new OpenFileDialog() {
				CheckFileExists = true,
				Multiselect = false,
				Filter = "MySQL Workbench Files (*.mwb)|*.mwb"
			};

			if (browse.ShowDialog() != true) {
				_Status.SetStatus("Canceled importing database.", ColorStatusBar.Status.Completed);
				return;
			}

			var generator = new MySqlMwbDdlGenerator(browse.FileName);

			var database = await generator.GenerateDdl();

			if (database != null) {
				_DatabaseExplorer.LoadDatabase(database);
				_Status.SetStatus("Completed model import.", ColorStatusBar.Status.Completed);
			}
		}


		private void BindCommand(ICommand command, KeyGesture gesture, ExecutedRoutedEventHandler execute) {
			BindCommand(command, gesture, execute, null);
		}

		private void BindCommand(ICommand command, KeyGesture gesture, ExecutedRoutedEventHandler execute, CanExecuteRoutedEventHandler can_execute) {
			
			if (gesture != null) {
				InputBindings.Add(new InputBinding(command, gesture));
			}
			
			CommandBinding cb = new CommandBinding(command);
			cb.Executed += execute;

			if (can_execute != null) {
				cb.CanExecute += can_execute;
			}

			CommandBindings.Add(cb);
		}

		private void Command_GenerateAll(object obSender, ExecutedRoutedEventArgs e) {
			_Status.SetStatus("Beginning Code Generation", ColorStatusBar.Status.Working);
			var database = _DatabaseExplorer.SelectedDatabase;
			var options = new ModelGenOptions(null) {
				OutputDbType = "sqlite",
				InputType = "ddl"
			};

			string base_ddl_filename = Path.Combine(Path.GetDirectoryName(database._FileLocation),
				Path.GetFileNameWithoutExtension(database._FileLocation));

			// If we are set to output in the ddl, then set a default name.
			if (database.GetConfiguration<bool>("output.sql_tables", false)) {
				options.SqlOutput = base_ddl_filename + ".sql";
			}

			// If we are set to output in the ddl, then set a default name.
			if (database.GetConfiguration<bool>("output.cs_classes", true)) {
				options.CodeOutput = base_ddl_filename + ".cs";
			}

			_Status.SetStatus("Generating Code...", ColorStatusBar.Status.Working);
			Program.ExecuteOptions(options, _DatabaseExplorer.SelectedDatabase);

			_Status.SetStatus("Completed Generating Code", ColorStatusBar.Status.Completed);
		}

		private void Command_GenerateAllCanExecute(object obSender, CanExecuteRoutedEventArgs e) {
			if (_DatabaseExplorer.SelectedType != ExplorerControl.Selection.None) {
				e.CanExecute = true;
			} else {
				e.CanExecute = false;
			}
		}

		private void Command_NewDatabase(object obSender, ExecutedRoutedEventArgs e) {
			_DatabaseExplorer.CreateDatabase();
			_Status.SetStatus("Created New Database", ColorStatusBar.Status.Completed);
			// Header="Generate All" Click="OutputGenerateAll_Click"
		}

		private void Command_Open(object obSender, ExecutedRoutedEventArgs e) {
			_Status.SetStatus("Opening Database", ColorStatusBar.Status.Working);
			if (_DatabaseExplorer.LoadDatabase() == false) {
				_Status.SetStatus("Canceled Opening Database", ColorStatusBar.Status.Completed);
			}
			
		}

		private async void Command_Import(object obSender, ExecutedRoutedEventArgs e) {
			_Status.SetStatus("Importing Database", ColorStatusBar.Status.Working);
			var db_server = new DatabaseServer(){ 
				Owner = this
			};

			if (db_server.ShowDialog() == true) {
				var database = await db_server.GetDatabase();
				if (database != null) {
					_DatabaseExplorer.LoadDatabase(database);
				}
				_Status.SetStatus("Completed Importing Database", ColorStatusBar.Status.Completed);
			} else {
				_Status.SetStatus("Canceled Importing Database", ColorStatusBar.Status.Completed);
			}
		}

		private void Command_Save(object obSender, ExecutedRoutedEventArgs e) {
			_DatabaseExplorer.Save();
			UpdateTitle();
		}

		private void Command_SaveCanExecute(object obSender, CanExecuteRoutedEventArgs e) {
			if (_DatabaseExplorer.SelectedType != ExplorerControl.Selection.None) {
				e.CanExecute = true;
			} else {
				e.CanExecute = false;
			}
		}

		private void Command_SaveAs(object obSender, ExecutedRoutedEventArgs e) {
			_DatabaseExplorer.Save(true);
			UpdateTitle();
		}

		private void Command_Exit(object obSender, ExecutedRoutedEventArgs e) {
			Exit(new System.ComponentModel.CancelEventArgs());
		}

		
		private void Exit(System.ComponentModel.CancelEventArgs e) {
			if (_DatabaseExplorer.CloseAllDatabases() == false) {
				e.Cancel = true;
			}
		}

		private Column GetSelectedColumn() {
			return _DagColumnDefinitions.SelectedItem as Column;
		}

		private Column[] GetSelectedColumns() {
			try {
				return _DagColumnDefinitions.SelectedItems.Cast<Column>().ToArray();
			} catch {
				return new Column[0];
			}
		}


		void TableColumn_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			Column column = sender as Column;
			_DatabaseExplorer.SelectedDatabase._Modified = true;

			// Temporarily remove this event so that we do not get stuck with a stack overflow.
			column.PropertyChanged -= TableColumn_PropertyChanged;


			switch (e.PropertyName) {
				case "DbType":
					column.NetType = type_transformer.DbToNetType(column.DbType);
					break;

				case "NetType":
					var type = type_transformer.NetType(column.NetType);
					column.DbLength = type.length;
					column.DbType = type.db_type;
					break;

				case "IsAutoIncrement":
					if (column.IsAutoIncrement) {

						if (new NetTypes[] { NetTypes.Int16, NetTypes.Int32, NetTypes.Int64 }.Contains(column.NetType) == false) {
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
			column.PropertyChanged += TableColumn_PropertyChanged;
		}


		private void _DatabaseExplorer_LoadedDatabase(object sender, ExplorerControl.DatabaseEventArgs e) {
			foreach (var table in e.Database.Table) {
				Utilities.BindChangedCollection<Column>(table.Column, null, TableColumn_PropertyChanged);
			}
			_Status.SetStatus("Opened Database", ColorStatusBar.Status.Completed);
		}


		private void _DatabaseExplorer_UnloadedDatabase(object sender, ExplorerControl.DatabaseEventArgs e) {
			_DagConfigurations.ItemsSource = null;
		}

		private void _DatabaseExplorer_ChangedSelection(object sender, ExplorerControl.SelectionChangedEventArgs e) {
			_DagColumnDefinitions.ItemsSource = null;
			_LstAssociations.ItemsSource = null;
			_TxtTableDescription.IsEnabled = false;
			_TxtTableDescription.Text = _TxtColumnDescription.Text = "";

			if (e.SelectionType != ExplorerControl.Selection.None) {
				_DagConfigurations.ItemsSource = e.Database.Configuration;
				switch (e.Database.TargetDb) {
					case DbProvider.Sqlite:
						type_transformer = new SqliteTypeTransformer();
						break;
					case DbProvider.MySQL:
						type_transformer = new MySqlTypeTransformer();
						break;
				}
				ColumnDbType.ItemsSource = type_transformer.DbTypes();
				_CmbTargetDatabase.DataContext = e.Database;
			}


			if (e.SelectionType == ExplorerControl.Selection.TableItem) {
				_DagColumnDefinitions.ItemsSource = e.Table.Column;
				_LstAssociations.ItemsSource = e.Database.Association;
				_tabTableSql.DataContext = e.Table;
				//_tabTable.IsSelected = true;

				_TxtTableDescription.IsEnabled = true;
				_TxtTableDescription.DataContext = e.Table;

				_LstAssociations.ItemsSource = e.Database.GetAssociations(e.Table);
			} else if (e.SelectionType == ExplorerControl.Selection.Database) {
				//_tabConfig.IsSelected = true;
			}

			UpdateTitle();
		}


		private void _DatabaseExplorer_DatabaseModified(object sender, ExplorerControl.DatabaseEventArgs e) {
			UpdateTitle();
		}

		private void UpdateTitle() {
			var database = _DatabaseExplorer.SelectedDatabase;

			if(database != null){
				if (database._FileLocation != null) {
					this.Title = "Dtronix Modeler - " + database.Name + " (" + Path.GetFileName(database._FileLocation) + ")";
				} else {
					this.Title = "Dtronix Modeler - " + database.Name + "(Usaved)";
				}

				if (database._Modified) {
					this.Title += " [Unsaved Changes]";
				}
			} else {
				this.Title = "Dtronix Modeler";
			}
		}

		private void _DagColumnDefinitions_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
			var column = GetSelectedColumn();
			_TxtColumnDescription.IsEnabled = _CmiMoveColumnDown.IsEnabled = _CmiMoveColumnUp.IsEnabled = false;
			
			if (column != null) {

				if (_DagColumnDefinitions.SelectedItems.Count == 1) {
					_CmiCreateAssociationWith.IsEnabled = true;
				} else {
					_CmiCreateAssociationWith.IsEnabled = false;
				}

				_CmiMoveColumnDown.IsEnabled = _CmiMoveColumnUp.IsEnabled = true;


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
				_DatabaseExplorer.SelectedTable.Column.Remove(column);
			}

		}

		private void _CmiMoveColumnUp_Click(object sender, RoutedEventArgs e) {
			var columns = GetSelectedColumns();
			var all_columns = _DatabaseExplorer.SelectedTable.Column;

			foreach (var column in columns) {

				int old_index = all_columns.IndexOf(column);

				if (old_index <= 0) {
					return;
				}

				all_columns.Move(old_index, old_index - 1);
			}
		}

		private void _CmiMoveColumnDown_Click(object sender, RoutedEventArgs e) {
			var columns = GetSelectedColumns();
			var all_columns = _DatabaseExplorer.SelectedTable.Column;

			foreach (var column in columns) {
				int old_index = all_columns.IndexOf(column);
				int max = all_columns.Count - 1;

				if (old_index >= max) {
					return;
				}

				all_columns.Move(old_index, old_index + 1);
			}

		}



		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			Exit(e);
		}


		private void _MiCommandlineToggle_Click(object sender, RoutedEventArgs e) {
			if (_MiCommandlineToggle.IsChecked) {
				Program.CommandlineShow();
			} else {
				Program.CommandlineHide();
			}
			
		}

		private void MenuItem_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
			_MiCommandlineToggle.IsChecked = Program.CommandlineVisible;
		}

		private void AssociationDelete_Click(object sender, RoutedEventArgs e) {
			var association = _LstAssociations.SelectedItem as Association;
			var database = _DatabaseExplorer.SelectedDatabase;

			var result = MessageBox.Show("Are you sure you want to delete the association " + association.DisplayName + "?", "Confirm", MessageBoxButton.YesNo);

			if (result == MessageBoxResult.Yes) {
				database.Association.Remove(association);
				_LstAssociations.ItemsSource = database.GetAssociations(_DatabaseExplorer.SelectedTable);
			}
		}

		private void AssociationCreate_Click(object sender, RoutedEventArgs e) {
			var association = new Association();
			var database = _DatabaseExplorer.SelectedDatabase;
			var column = GetSelectedColumn();

			if (_DatabaseExplorer.SelectedTable != null) {
				association.Table1 = _DatabaseExplorer.SelectedTable.Name;
			}

			if (column != null) {
				association.Table1Column = column.Name;
			}

			if (_DatabaseExplorer.SelectedTable != null) {
				association.Table1 = _DatabaseExplorer.SelectedTable.Name;
			}

			// See if we can determine what other table and column this column is referencing.
			if (column != null && column.Name.Contains('_')) {
				int index = column.Name.IndexOf('_');
				string sel_table = column.Name.Substring(0, index);
				string sel_column = column.Name.Substring(index + 1);
				DtxModeler.Ddl.Table found_table = null;

				if ((found_table = database.Table.FirstOrDefault(t => t.Name == sel_table)) != null) {
					association.Table2 = sel_table;

					if (found_table.Column.FirstOrDefault(c => c.Name == sel_column) != null) {
						association.Table2Column = sel_column;
						association.Table1Cardinality = Cardinality.Many;
						association.Table2Cardinality = Cardinality.One;
					}
				}
			}


			var association_window = new AssociationWindow(database, association);
			association_window.Owner = this;

			if (association_window.ShowDialog() == true) {
				database.Association.Add(association_window.Association);
				_LstAssociations.ItemsSource = database.GetAssociations(_DatabaseExplorer.SelectedTable);
			}
		}

		private void AssociationEdit_Click(object sender, RoutedEventArgs e) {
			var original_association = _LstAssociations.SelectedItem as Association;
			var database = _DatabaseExplorer.SelectedDatabase;

			var association_window = new AssociationWindow(database, original_association.Clone());
			association_window.Owner = this;

			if (association_window.ShowDialog() == true) {
				database.Association.Remove(original_association);
				database.Association.Add(association_window.Association);
				_LstAssociations.ItemsSource = database.GetAssociations(_DatabaseExplorer.SelectedTable);
			}
		}

		private void _LstAssociations_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
			_CmiCreateAssociation.IsEnabled = _CmiDeleteAssociation.IsEnabled = _CmiEditAssociation.IsEnabled = false;

			if (_DatabaseExplorer.SelectedTable != null) {
				_CmiCreateAssociation.IsEnabled = true;
			}

			var association = _LstAssociations.SelectedItem as Association;

			if (association != null) {
				_CmiDeleteAssociation.IsEnabled = _CmiEditAssociation.IsEnabled = true;
			}
		}

		private void _DagColumnDefinitions_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var columns = GetSelectedColumns();
			if (columns.Length == 1) {
				_TxtColumnDescription.IsEnabled = true;
				_TxtColumnDescription.DataContext = columns[0];
			} else {
				_TxtColumnDescription.IsEnabled = false;
				_TxtColumnDescription.DataContext = null;
			}

		}

		private void Window_KeyUp(object sender, KeyEventArgs e) {
			if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.S) {
				_DatabaseExplorer.Save();
				UpdateTitle();
			}
		}

		private void _DagColumnDefinitions_PasteCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			// Only allow paste if the clipboard is valid XML.
			if (Clipboard.ContainsText()){ 
				e.CanExecute = true;
			} else {
				e.CanExecute = false;
				e.Handled = true;
			}
		}


		private void _DagColumnDefinitions_Paste(object sender, ExecutedRoutedEventArgs e) {
			Column[] columns = Utilities.XmlDeserializeString<DtxModeler.Ddl.Column[]>(Clipboard.GetText()) as Column[];
			var all_columns = _DatabaseExplorer.SelectedTable.Column;

			if (columns == null) {
				return;
			}

			if (_MiValidateColumnsOnPaste.IsChecked) {
				foreach (var column in columns) {
					var found_column = all_columns.FirstOrDefault(col => col.Name.ToLower() == column.Name.ToLower());

					if (found_column != null) {
						InputDialogBox.Show("Column Naming Collision", "Enter a new name for the old \"" + found_column.Name + "\" Column.", found_column.Name, value => {
							column.Name = value;
						});

						continue;
					}
				}
			}

			// Add in reverse order to allow for insertion in logical order.
			for (int i = columns.Length - 1; i >= 0; i--) {
				if (_DagColumnDefinitions.Items.Count > _DagColumnDefinitions.SelectedIndex + 1) {
					_DatabaseExplorer.SelectedTable.Column.Insert(_DagColumnDefinitions.SelectedIndex + 1, columns[i]);
				}else{
					_DatabaseExplorer.SelectedTable.Column.Add(columns[i]);
				}
			}
		}

		private void _DagColumnDefinitions_CopyCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			if (GetSelectedColumn() != null) {
				e.CanExecute = true;
			} else {
				e.CanExecute = false;
				e.Handled = true;
			}
		}

		private void _DagColumnDefinitions_Copy(object sender, ExecutedRoutedEventArgs e) {
			var text = Utilities.XmlSerializeObject(GetSelectedColumns());
			Clipboard.SetText(text, TextDataFormat.UnicodeText);
		}

		private void _TxtConfigSearch_TextChanged(object sender, TextChangedEventArgs e) {
			var database = _DatabaseExplorer.SelectedDatabase;

			if (database == null || database.Configuration == null || _TxtConfigSearch.Text == " Search Configurations") {
				return;
			}

			var text = _TxtConfigSearch.Text;
			var text_empty = string.IsNullOrWhiteSpace(_TxtConfigSearch.Text);

			foreach (var config in database.Configuration) {
				if (config.Name.Contains(text) == false && text_empty == false) {
					config.Visibility = System.Windows.Visibility.Collapsed;
				} else {
					config.Visibility = System.Windows.Visibility.Visible;
				}
			}
		}

		private void _TxtConfigSearch_GotFocus(object sender, RoutedEventArgs e) {
			if (_TxtConfigSearch.Text == " Search Configurations") {
				_TxtConfigSearch.Text = "";
			}
		}

		private void _TxtConfigSearch_LostFocus(object sender, RoutedEventArgs e) {
			if (string.IsNullOrWhiteSpace(_TxtConfigSearch.Text)) {
				_TxtConfigSearch.Text = " Search Configurations";
			}
		}

		private void _DagConfigurations_Hyperlink(object sender, RoutedEventArgs e) {
			Hyperlink link = e.OriginalSource as Hyperlink;
			if (link.NavigateUri.IsAbsoluteUri) {
				Process.Start(link.NavigateUri.ToString());
			}
		} 


	}

}
