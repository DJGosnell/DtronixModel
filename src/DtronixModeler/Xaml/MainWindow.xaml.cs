using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.Windows.Documents;
using System.Diagnostics;
using DtronixModeler.Generator;
using DtronixModeler.Generator.Ddl;
using DtronixModeler.MySql;
using DtronixModeler.MySqlMwb;
using DtronixModeler.Sqlite;
using Association = DtronixModeler.Generator.Ddl.Association;
using Column = DtronixModeler.Generator.Ddl.Column;
using Table = DtronixModeler.Generator.Ddl.Table;

namespace DtronixModeler.Xaml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TypeTransformer type_transformer;

        private Column previous_column = null;

        private List<string> DefaultNetTypes;

        public MainWindow() : this(null)
        {

        }

        public ObservableCollection<string> NetTypes { get; set; }

        public MainWindow(string open_file)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            

            DefaultNetTypes = new List<string>
            {
                "Int16",
                "UInt16",
                "Int32",
                "UInt32",
                "Int64",
                "UInt64",
                "ByteArray",
                "SByte",
                "Byte",
                "DateTimeOffset",
                "Decimal",
                "Float",
                "Double",
                "Boolean",
                "String",
                "Char",
            };

            _CmbTargetDatabase.ItemsSource = Enum.GetValues(typeof(DbProvider)).Cast<DbProvider>();

            BindCommand(Commands.NewDatabase, null, Command_NewDatabase);
            BindCommand(ApplicationCommands.Open, new KeyGesture(Key.O, ModifierKeys.Control), Command_Open);
            BindCommand(Commands.ImportDatabase, new KeyGesture(Key.I, ModifierKeys.Control), Command_Import);
            BindCommand(Commands.ImportMySqlMwb, null, Command_ImportMySqlMwb);
            BindCommand(ApplicationCommands.Save, new KeyGesture(Key.S, ModifierKeys.Control), Command_Save,
                Command_SaveCanExecute);
            BindCommand(ApplicationCommands.SaveAs, new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Alt),
                Command_SaveAs, Command_SaveCanExecute);
            BindCommand(Commands.Exit, new KeyGesture(Key.F4, ModifierKeys.Alt), Command_Exit);
            BindCommand(Commands.GenerateAll, new KeyGesture(Key.F5), Command_GenerateAll,
                Command_GenerateAllCanExecute);
            BindCommand(Commands.About, null, (sender, e) => new AboutWindow().ShowDialog(),
                (sender, e) => e.CanExecute = true);

            _Status.SetStatus("Application Loaded And Ready", ColorStatusBar.Status.Completed);

            if (open_file != null)
            {
                if (_DatabaseExplorer.LoadDatabase(open_file) == false)
                {
                    _Status.SetStatus("Could not load database file specified: " + open_file,
                        ColorStatusBar.Status.Error);
                }
            }
        }

        private async void Command_ImportMySqlMwb(object sender, ExecutedRoutedEventArgs e)
        {
            _Status.SetStatus("Importing MySQL Workbench model.", ColorStatusBar.Status.Working);
            var browse = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = "MySQL Workbench Files (*.mwb)|*.mwb"
            };

            if (browse.ShowDialog() != true)
            {
                _Status.SetStatus("Canceled importing database.", ColorStatusBar.Status.Completed);
                return;
            }

            var generator = new MySqlMwbDdlGenerator(browse.FileName, false);

            var database = await generator.GenerateDdl();

            if (database != null)
            {
                _DatabaseExplorer.LoadDatabase(database);
                _Status.SetStatus("Completed model import.", ColorStatusBar.Status.Completed);
            }
        }


        private void BindCommand(ICommand command, KeyGesture gesture, ExecutedRoutedEventHandler execute)
        {
            BindCommand(command, gesture, execute, null);
        }

        private void BindCommand(ICommand command, KeyGesture gesture, ExecutedRoutedEventHandler execute,
            CanExecuteRoutedEventHandler can_execute)
        {

            if (gesture != null)
            {
                InputBindings.Add(new InputBinding(command, gesture));
            }

            CommandBinding cb = new CommandBinding(command);
            cb.Executed += execute;

            if (can_execute != null)
            {
                cb.CanExecute += can_execute;
            }

            CommandBindings.Add(cb);
        }

        private void Command_GenerateAll(object obSender, ExecutedRoutedEventArgs e)
        {
            _Status.SetStatus("Beginning Code Generation", ColorStatusBar.Status.Working);
            var database = _DatabaseExplorer.SelectedDatabase;
            var options = new CommandOptions
            {
                DbProvider = database.TargetDb,
                InputType = CommandOptions.InType.Ddl,
                ProtobufDataContracts = database.ImplementProtobufNetDataContracts,
                MessagePackAttributes = database.ImplementMessagePackAttributes,
                SystemTextJsonAttributes = database.ImplementSystemTextJsonAttributes,
                DataContractMemberOrder = database.ImplementDataContractMemberOrder,
                DataContractMemberName = database.ImplementDataContractMemberName,
                NotifyPropertyChanged = database.ImplementINotifyPropertyChanged,
                ProtobufPackage = database.ProtobufPackage
            };

            string base_ddl_filename = Path.Combine(Path.GetDirectoryName(database._FileLocation),
                Path.GetFileNameWithoutExtension(database._FileLocation));

            // If we are set to output in the ddl, then set a default name.
            if (database.OutputSqlTables)
            {
                options.SqlOutput = base_ddl_filename + ".sql";
            }

            // If we are set to output in the ddl, then set a default name.
            if (database.OutputCsClasses)
            {
                options.CodeOutput = base_ddl_filename + ".cs";
            }

            // If we are set to output in the ddl, then set a default name.
            if (database.OutputProtobuf)
            {
                options.ProtobufOutput = base_ddl_filename + ".proto";
            }

            _Status.SetStatus("Generating Code...", ColorStatusBar.Status.Working);


            try
            {
                Task.Run(async () =>
                {
                    await Program.ExecuteOptions(options, database);
                }).Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Error in executing specified options. Error:");
                foreach (var inner_ex in ex.InnerExceptions)
                {
                    Console.WriteLine(inner_ex.Message);
                    Console.WriteLine(inner_ex.ToString());
                }
            }


            _Status.SetStatus("Completed Generating Code", ColorStatusBar.Status.Completed);
        }

        private void Command_GenerateAllCanExecute(object obSender, CanExecuteRoutedEventArgs e)
        {
            if (_DatabaseExplorer.SelectedType != ExplorerControl.Selection.None)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void Command_NewDatabase(object obSender, ExecutedRoutedEventArgs e)
        {
            _DatabaseExplorer.CreateDatabase();
            _Status.SetStatus("Created New Database", ColorStatusBar.Status.Completed);
            // Header="Generate All" Click="OutputGenerateAll_Click"
        }

        private void Command_Open(object obSender, ExecutedRoutedEventArgs e)
        {
            _Status.SetStatus("Opening Database", ColorStatusBar.Status.Working);
            if (_DatabaseExplorer.LoadDatabase() == false)
            {
                _Status.SetStatus("Canceled Opening Database", ColorStatusBar.Status.Completed);
            }

        }

        private async void Command_Import(object obSender, ExecutedRoutedEventArgs e)
        {
            _Status.SetStatus("Importing Database", ColorStatusBar.Status.Working);
            var db_server = new DatabaseServer
            {
                Owner = this
            };

            if (db_server.ShowDialog() == true)
            {
                var database = await db_server.GetDatabase();
                if (database != null)
                {
                    _DatabaseExplorer.LoadDatabase(database);
                }
                _Status.SetStatus("Completed Importing Database", ColorStatusBar.Status.Completed);
            }
            else
            {
                _Status.SetStatus("Canceled Importing Database", ColorStatusBar.Status.Completed);
            }
        }

        private void Command_Save(object obSender, ExecutedRoutedEventArgs e)
        {
            _DatabaseExplorer.Save();
            UpdateTitle();
        }

        private void Command_SaveCanExecute(object obSender, CanExecuteRoutedEventArgs e)
        {
            if (_DatabaseExplorer.SelectedType != ExplorerControl.Selection.None)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void Command_SaveAs(object obSender, ExecutedRoutedEventArgs e)
        {
            _DatabaseExplorer.Save(true);
            UpdateTitle();
        }

        private void Command_Exit(object obSender, ExecutedRoutedEventArgs e)
        {
            Exit(new CancelEventArgs());
        }


        private void Exit(CancelEventArgs e)
        {
            if (_DatabaseExplorer.CloseAllDatabases() == false)
            {
                e.Cancel = true;
            }
        }

        private Column GetSelectedColumn()
        {
            return _DagColumnDefinitions.SelectedItem as Column;
        }

        private Column[] GetSelectedColumns()
        {
            try
            {
                return _DagColumnDefinitions.SelectedItems.Cast<Column>().ToArray();
            }
            catch
            {
                return new Column[0];
            }
        }


        void TableColumn_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Column column = sender as Column;
            _DatabaseExplorer.SelectedDatabase._Modified = true;

            // Temporarily remove this event so that we do not get stuck with a stack overflow.
            column.PropertyChanged -= TableColumn_PropertyChanged;


            switch (e.PropertyName)
            {
                case "Name":
                    ColumnReservedWords reserved_word_match;

                    if (Enum.TryParse(column.Name, true, out reserved_word_match))
                    {
                        column.Name = "FIX_COLUMN_NAME";
                        MessageBox.Show(
                            "Can not use reserved word '" + reserved_word_match.ToString() + "' for a column name.",
                            "Column Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        column.PostRename(_DatabaseExplorer.SelectedDatabase, previous_column.Name);
                    }
                    break;

                case "DbType":
                    column.NetType = type_transformer.DbToNetType(column.DbType, column.IsUnsigned);
                    break;

                case "NetType":
                    try
                    {
                        var type = type_transformer.NetType(column.NetType);

                        if (type == null && NetTypes.Contains(column.NetType))
                        {
                            type = type_transformer.NetType("Int32");
                        }

                        column.DbLength = type.Length;
                        column.DbType = type.DbType;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Type is not allowed in the selected database type.", "Invalid Option");
                    }

                    break;

                case "IsUnsigned":
                    if (column.NetType.Contains("Int") || column.NetType.Contains("Byte"))
                    {
                        TableColumn_PropertyChanged(sender, new PropertyChangedEventArgs("NetType"));
                        TableColumn_PropertyChanged(sender, new PropertyChangedEventArgs("DbType"));
                    }
                    else
                    {
                        column.IsUnsigned = false;
                        MessageBox.Show(
                            "Can not change unsigned value for non-number column",
                            "Column Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    break;

                case "IsAutoIncrement":
                    if (column.IsAutoIncrement)
                    {

                        if (new string[] { "Int16", "Int32", "Int64" }.Contains(column.NetType) == false)
                        {
                            MessageBox.Show("An auto incremented column has to be an integer type.", "Invalid Option");
                            column.IsAutoIncrement = false;
                        }
                        else if (column.Nullable)
                        {
                            MessageBox.Show("Can not auto increment a column that is nullable.", "Invalid Option");
                            column.IsAutoIncrement = false;
                        }
                    }
                    break;

                case "Nullable":
                    if (column.Nullable && column.IsAutoIncrement)
                    {
                        MessageBox.Show("Can not make an auto incremented value nullable.", "Invalid Option");
                        column.Nullable = false;
                    }

                    if (column.Nullable && DefaultNetTypes.Contains(column.NetType) == false &&
                        NetTypes.Contains(column.NetType))
                    {
                        MessageBox.Show("Can not make an enum value nullable.", "Invalid Option");
                        column.Nullable = false;
                    }
                    break;

                case "DefaultValue":
                    if (column.IsAutoIncrement)
                    {
                        MessageBox.Show("An auto incremented value can not have a default value.", "Invalid Option");
                        column.DefaultValue = null;
                    }
                    break;
            }

            previous_column = column.Clone();


            // Rebind this event to allow us to listen again.
            column.PropertyChanged += TableColumn_PropertyChanged;
        }


        private void _DatabaseExplorer_LoadedDatabase(object sender, ExplorerControl.DatabaseEventArgs e)
        {
            foreach (var table in e.Database.Table)
            {
                Utilities.BindChangedCollection<Column>(table.Column, null, TableColumn_PropertyChanged);
            }
            _Status.SetStatus("Opened Database", ColorStatusBar.Status.Completed);
        }


        private void _DatabaseExplorer_UnloadedDatabase(object sender, ExplorerControl.DatabaseEventArgs e)
        {
            _DagConfigurations.ItemsSource = null;
        }

        private void _DatabaseExplorer_ChangedSelection(object sender, ExplorerControl.SelectionChangedEventArgs e)
        {
            _DagColumnDefinitions.ItemsSource = null;
            _DagTableAssociations.ItemsSource = null;
            _TxtTableDescription.IsEnabled = false;
            _TxtTableDescription.Text = _TxtColumnDescription.Text = "";

            if (e.SelectionType != ExplorerControl.Selection.None)
            {
                _DagConfigurations.ItemsSource = e.Database.Configuration;
                switch (e.Database.TargetDb)
                {
                    case DbProvider.Sqlite:
                        type_transformer = new SqliteTypeTransformer();
                        break;
                    case DbProvider.MySQL:
                        type_transformer = new MySqlTypeTransformer();
                        break;
                }
                ColumnDbType.ItemsSource = type_transformer.DbTypes();
                DataContext = e.Database;

                NetTypes = new ObservableCollection<string>(DefaultNetTypes);

                foreach (var type in e.Database.Enumeration)
                {
                    NetTypes.Add(type.Name);
                }

                ColumnNetType.ItemsSource = NetTypes;
                //_TabEnums.DataContext = e.Database.Enumeration;
            }


            if (e.SelectionType == ExplorerControl.Selection.TableItem)
            {
                _DagColumnDefinitions.ItemsSource = e.Table.Column;
                _tabTableSql.DataContext = e.Table;
                //_tabTable.IsSelected = true;

                _TxtTableDescription.IsEnabled = true;
                _TxtTableDescription.DataContext = e.Table;

                _DagTableAssociations.ItemsSource = e.Database.GetAssociations(e.Table);
            }
            else if (e.SelectionType == ExplorerControl.Selection.Database)
            {
                //_tabConfig.IsSelected = true;
            }



            UpdateTitle();
        }


        private void _DatabaseExplorer_DatabaseModified(object sender, ExplorerControl.DatabaseEventArgs e)
        {
            // TODO: Investigate new method of checking for changes in the document.
            //Dispatcher.Invoke(UpdateTitle);

        }

        private void UpdateTitle()
        {
            var database = _DatabaseExplorer.SelectedDatabase;

            if (database != null)
            {
                if (database._FileLocation != null)
                {
                    Title = "Dtronix Modeler - " + database.Name + " (" + Path.GetFileName(database._FileLocation) +
                            ")";
                }
                else
                {
                    Title = "Dtronix Modeler - " + database.Name + "(Usaved)";
                }

                if (database._Modified)
                {
                    Title += " [Unsaved Changes]";
                }
            }
            else
            {
                Title = "Dtronix Modeler";
            }
        }

        private void _DagColumnDefinitions_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var column = GetSelectedColumn();
            _TxtColumnDescription.IsEnabled = _CmiMoveColumnDown.IsEnabled = _CmiMoveColumnUp.IsEnabled = false;

            if (column != null)
            {

                if (_DagColumnDefinitions.SelectedItems.Count == 1)
                {
                    _CmiCreateAssociationWith.IsEnabled = true;
                }
                else
                {
                    _CmiCreateAssociationWith.IsEnabled = false;
                }

                _CmiMoveColumnDown.IsEnabled = _CmiMoveColumnUp.IsEnabled = true;


            }
        }


        private void _CmiDeleteColumn_Click(object sender, RoutedEventArgs e)
        {
            var columns = GetSelectedColumns();
            string text_columns = "";
            foreach (var column in columns)
            {
                text_columns += column.Name + "\r\n";
            }

            var result = MessageBox.Show("Are you sure you want to delete the following columns? \r\n" + text_columns,
                "Confirm Column Deletion", MessageBoxButton.YesNo);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            foreach (var column in columns)
            {
                _DatabaseExplorer.SelectedTable.Column.Remove(column);
            }

        }

        private void _CmiMoveColumnUp_Click(object sender, RoutedEventArgs e)
        {
            var columns = GetSelectedColumns();
            var all_columns = _DatabaseExplorer.SelectedTable.Column;

            foreach (var column in columns)
            {

                int old_index = all_columns.IndexOf(column);

                if (old_index <= 0)
                {
                    return;
                }

                all_columns.Move(old_index, old_index - 1);
            }
        }

        private void _CmiMoveColumnDown_Click(object sender, RoutedEventArgs e)
        {
            var columns = GetSelectedColumns();
            var all_columns = _DatabaseExplorer.SelectedTable.Column;

            foreach (var column in columns)
            {
                int old_index = all_columns.IndexOf(column);
                int max = all_columns.Count - 1;

                if (old_index >= max)
                {
                    return;
                }

                all_columns.Move(old_index, old_index + 1);
            }

        }



        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Exit(e);
        }


        private void _MiCommandlineToggle_Click(object sender, RoutedEventArgs e)
        {
            if (_MiCommandlineToggle.IsChecked)
            {
                Program.CommandlineShow();
            }
            else
            {
                Program.CommandlineHide();
            }

        }

        private void MenuItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            _MiCommandlineToggle.IsChecked = Program.CommandlineVisible;
        }

        private void AssociationDelete_Click(object sender, RoutedEventArgs e)
        {
            var association = _DagTableAssociations.SelectedItem as Association;
            var database = _DatabaseExplorer.SelectedDatabase;

            var result =
                MessageBox.Show("Are you sure you want to delete the association " + association.DisplayName + "?",
                    "Confirm", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                database.Association.Remove(association);
                _DagTableAssociations.ItemsSource = database.GetAssociations(_DatabaseExplorer.SelectedTable);
            }
        }

        private void AssociationCreate_Click(object sender, RoutedEventArgs e)
        {
            var association = new Association();
            var database = _DatabaseExplorer.SelectedDatabase;
            var column = GetSelectedColumn();

            if (_DatabaseExplorer.SelectedTable != null)
            {
                association.Table1 = _DatabaseExplorer.SelectedTable.Name;
            }

            if (column != null)
            {
                association.Table1Column = column.Name;
            }

            if (_DatabaseExplorer.SelectedTable != null)
            {
                association.Table1 = _DatabaseExplorer.SelectedTable.Name;
            }

            // See if we can determine what other table and column this column is referencing.
            if (column != null && column.Name.Contains('_'))
            {
                int index = column.Name.IndexOf('_');
                string sel_table = column.Name.Substring(0, index);
                string sel_column = column.Name.Substring(index + 1);
                Table found_table = null;

                if ((found_table = database.Table.FirstOrDefault(t => t.Name == sel_table)) != null)
                {
                    association.Table2 = sel_table;

                    if (found_table.Column.FirstOrDefault(c => c.Name == sel_column) != null)
                    {
                        association.Table2Column = sel_column;
                        association.Table1Cardinality = Cardinality.Many;
                        association.Table2Cardinality = Cardinality.One;
                    }
                }
            }


            var association_window = new AssociationWindow(database, association);
            association_window.Owner = this;

            if (association_window.ShowDialog() == true)
            {
                database.Association.Add(association_window.Association);
                _DagTableAssociations.ItemsSource = database.GetAssociations(_DatabaseExplorer.SelectedTable);
            }
        }

        private void AssociationEdit_Click(object sender, RoutedEventArgs e)
        {
            var original_association = _DagTableAssociations.SelectedItem as Association;
            var database = _DatabaseExplorer.SelectedDatabase;

            var association_window = new AssociationWindow(database, original_association.Clone());
            association_window.Owner = this;

            if (association_window.ShowDialog() == true)
            {
                database.Association.Remove(original_association);
                database.Association.Add(association_window.Association);
                _DagTableAssociations.ItemsSource = database.GetAssociations(_DatabaseExplorer.SelectedTable);
            }
        }

        private void _DagTableAssociations_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            _CmiCreateAssociation.IsEnabled = _CmiDeleteAssociation.IsEnabled = _CmiEditAssociation.IsEnabled = false;

            if (_DatabaseExplorer.SelectedTable != null)
            {
                _CmiCreateAssociation.IsEnabled = true;
            }

            var association = _DagTableAssociations.SelectedItem as Association;

            if (association != null)
            {
                _CmiDeleteAssociation.IsEnabled = _CmiEditAssociation.IsEnabled = true;
            }
        }

        private void _DagColumnDefinitions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var columns = GetSelectedColumns();
            if (columns.Length == 1)
            {
                _TxtColumnDescription.IsEnabled = true;
                _TxtColumnDescription.DataContext = columns[0];
                previous_column = columns[0].Clone();
            }
            else
            {
                _TxtColumnDescription.IsEnabled = false;
                _TxtColumnDescription.DataContext = null;
            }

        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && e.Key == Key.S)
            {
                _DatabaseExplorer.Save();
                UpdateTitle();
            }
        }

        private void _DagColumnDefinitions_PasteCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // Only allow paste if the clipboard is valid XML.
            if (Clipboard.ContainsText())
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
                e.Handled = true;
            }
        }


        private void _DagColumnDefinitions_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            Column[] columns = Utilities.XmlDeserializeString<Column[]>(Clipboard.GetText()) as Column[];
            var all_columns = _DatabaseExplorer.SelectedTable.Column;

            if (columns == null)
            {
                return;
            }

            if (_MiValidateColumnsOnPaste.IsChecked)
            {
                foreach (var column in columns)
                {
                    var found_column = all_columns.FirstOrDefault(col => col.Name.ToLower() == column.Name.ToLower());

                    if (found_column != null)
                    {
                        InputDialogBox.Show("Column Naming Collision",
                            "Enter a new name for the old \"" + found_column.Name + "\" Column.", found_column.Name,
                            value =>
                            {
                                column.Name = value;
                            });

                        continue;
                    }
                }
            }

            // Add in reverse order to allow for insertion in logical order.
            for (int i = columns.Length - 1; i >= 0; i--)
            {
                if (_DagColumnDefinitions.Items.Count > _DagColumnDefinitions.SelectedIndex + 1)
                {
                    _DatabaseExplorer.SelectedTable.Column.Insert(_DagColumnDefinitions.SelectedIndex + 1, columns[i]);
                }
                else
                {
                    _DatabaseExplorer.SelectedTable.Column.Add(columns[i]);
                }
            }
        }

        private void _DagColumnDefinitions_CopyCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (GetSelectedColumn() != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
                e.Handled = true;
            }

        }

        private void _DagColumnDefinitions_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            var text = Utilities.XmlSerializeObject(GetSelectedColumns());
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
        }

        private void _TxtConfigSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO: Review implications of removal.
            /*
            var database = _DatabaseExplorer.SelectedDatabase;

            if (database == null || database.Configuration == null || _TxtConfigSearch.Text == " Search Configurations")
            {
                return;
            }

            var text = _TxtConfigSearch.Text;
            var text_empty = string.IsNullOrWhiteSpace(_TxtConfigSearch.Text);

            
            foreach (var config in database.Configuration)
            {
                if (config.Name.Contains(text) == false && text_empty == false)
                {
                    config.Visibility = Visibility.Collapsed;
                }
                else
                {
                    config.Visibility = Visibility.Visible;
                }
            }*/
        }

        private void _TxtConfigSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_TxtConfigSearch.Text == " Search Configurations")
            {
                _TxtConfigSearch.Text = "";
            }
        }

        private void _TxtConfigSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_TxtConfigSearch.Text))
            {
                _TxtConfigSearch.Text = " Search Configurations";
            }
        }

        private void _DagConfigurations_Hyperlink(object sender, RoutedEventArgs e)
        {
            Hyperlink link = e.OriginalSource as Hyperlink;
            if (link.NavigateUri.IsAbsoluteUri)
            {
                Process.Start(link.NavigateUri.ToString());
            }
        }

        private void NewEnum(object sender, ExecutedRoutedEventArgs e)
        {
            InputDialogBox.Show("New Enum Name", "Enter a name for the Enum collection.", "", value =>
            {
                var new_enum = new Enumeration();
                new_enum.Name = value;
                _DatabaseExplorer.SelectedDatabase.Enumeration.Add(new_enum);
                NetTypes.Add(value);
            });
        }


        private void DeleteEnum(object sender, ExecutedRoutedEventArgs e)
        {
            var selected_enum = _LstEnums.SelectedValue as Enumeration;
            _DatabaseExplorer.SelectedDatabase.Enumeration.Remove(selected_enum);
            NetTypes.Remove(selected_enum.Name);
        }

        private void NewEnum_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_DatabaseExplorer.SelectedDatabase != null)
            {
                e.CanExecute = true;
            }
        }

        private void DeleteEnum_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_LstEnums.SelectedValue != null)
            {
                e.CanExecute = true;
            }
        }

        private void MoveEnumUp(object sender, ExecutedRoutedEventArgs e)
        {
            var data_grid = e.Source as DataGrid;
            var list = data_grid.ItemsSource as ObservableCollection<EnumValue>;
            int new_index = (data_grid.SelectedIndex > 1) ? data_grid.SelectedIndex - 1 : 0;

            list.Move(data_grid.SelectedIndex, new_index);
        }

        private void MoveEnumDown(object sender, ExecutedRoutedEventArgs e)
        {
            var data_grid = e.Source as DataGrid;
            var list = data_grid.ItemsSource as ObservableCollection<EnumValue>;
            int new_index = (data_grid.SelectedIndex < list.Count - 1) ? data_grid.SelectedIndex + 1 : list.Count - 1;

            list.Move(data_grid.SelectedIndex, new_index);
        }

        private void DeleteEnumValue(object sender, ExecutedRoutedEventArgs e)
        {
            var data_grid = e.Source as DataGrid;
            var list = data_grid.ItemsSource as ObservableCollection<EnumValue>;
            list.RemoveAt(data_grid.SelectedIndex);
        }

        private void ModifyEnumValue_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var data_grid = e.Source as DataGrid;
            if (data_grid != null)
            {
                if (data_grid.SelectedValue is EnumValue)
                {
                    e.CanExecute = true;
                }
            }
        }
    }

}
