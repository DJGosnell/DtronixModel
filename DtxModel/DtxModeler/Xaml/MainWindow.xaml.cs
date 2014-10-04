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

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private object deserialized_clipboard;

		private TypeTransformer type_transformer = new SqliteTypeTransformer();
		

		public MainWindow() {
			InitializeComponent();

			ColumnDbType.ItemsSource = type_transformer.DbTypes();

			ColumnNetType.ItemsSource = Enum.GetValues(typeof(NetTypes)).Cast<NetTypes>();
		}

		private Column GetSelectedColumn() {
			return _dagColumnDefinitions.SelectedItem as Column;
			
		}

		private Column[] GetSelectedColumns() {
			return _dagColumnDefinitions.SelectedItems.Cast<Column>().ToArray();
		}



		private void New_Click(object sender, RoutedEventArgs e) {
			_DatabaseExplorer.CreateDatabase();
		}

		private void Open_Click(object sender, RoutedEventArgs e) {
			_DatabaseExplorer.LoadDatabase();
		}

		private void Exit_Click(object sender, RoutedEventArgs e) {
			Exit(new System.ComponentModel.CancelEventArgs());
		}

		private void Save_Click(object sender, RoutedEventArgs e) {
			_DatabaseExplorer.Save();
		}

		private void SaveAs_Click(object sender, RoutedEventArgs e) {
			_DatabaseExplorer.Save(true);
		}

		void Column_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			Column column = sender as Column;
			_DatabaseExplorer.SelectedDatabase._Modified = true;

			// Temporarily remove this event so that we do not get stuck with a stack overflow.
			column.PropertyChanged -= Column_PropertyChanged;


			switch (e.PropertyName) {
				case "DbType":
					column.NetType = type_transformer.DbToNetType(column.DbType);
					break;

				case "NetType":
					column.DbType = type_transformer.NetToDbType(column.NetType);
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
			column.PropertyChanged += Column_PropertyChanged;
		}


		private void _DatabaseExplorer_ChangedSelection(object sender, ExplorerControl.SelectionChangedEventArgs e) {
			_dagColumnDefinitions.ItemsSource = null;
			_TxtTableDescription.IsEnabled = _TxtColumnDescription.IsEnabled = false;
			_TxtTableDescription.Text = _TxtColumnDescription.Text = "";

			if (e.SelectionType == ExplorerControl.Selection.TableItem) {
				_dagColumnDefinitions.ItemsSource = e.Table.Column;
				_tabTable.IsSelected = true;

				_TxtTableDescription.IsEnabled = true;
				_TxtTableDescription.DataContext = e.Table;

			} else if (e.SelectionType == ExplorerControl.Selection.Database) {
				_tabConfig.IsSelected = true;
			}
			
		}

		private void _DatabaseExplorer_LoadedDatabase(object sender, ExplorerControl.LoadedDatabaseEventArgs e) {
			foreach (var table in e.Database.Table) {

				// Attach events to all existing columns.
				foreach (Column column in table.Column) {
					column.PropertyChanged += Column_PropertyChanged;
				}

				// Ensure that the events are attached to all future columns.
				table.Column.CollectionChanged += ((coll_sender, coll_e) => {

					if (coll_e.Action == NotifyCollectionChangedAction.Add) {
						_DatabaseExplorer.SelectedDatabase._Modified = true;

						// If we add a new column, add a new property changed event to it.
						foreach (Column column in coll_e.NewItems) {
							column.PropertyChanged += Column_PropertyChanged;
						}
					}


				});
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
				if (Clipboard.ContainsText() && (deserialized_clipboard = Utilities.XmlDeserializeString<DtxModeler.Ddl.Column[]>(Clipboard.GetText())) != null) {
					_CmiPasteColumn.IsEnabled = true;
				}

			} else {
				if (_dagColumnDefinitions.SelectedItems.Count > 1) {

				}
				_TxtColumnDescription.IsEnabled = _CmiDeleteColumn.IsEnabled = _CmiCopyColumn.IsEnabled = _CmiPasteColumn.IsEnabled = _CmiMoveColumnDown.IsEnabled = _CmiMoveColumnUp.IsEnabled = false;
			}
		}


		private void _TxtColumnDescription_TextChanged(object sender, TextChangedEventArgs e) {
			var column = GetSelectedColumn();
			if (column != null) {
				column.Description = _TxtColumnDescription.Text;
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

		private void _CmiCopyColumn_Click(object sender, RoutedEventArgs e) {
			var text = Utilities.XmlSerializeObject(GetSelectedColumns());
			Clipboard.SetText(text, TextDataFormat.UnicodeText);
		}

		private void _CmiPasteColumn_Click(object sender, RoutedEventArgs e) {
			Column[] columns = deserialized_clipboard as Column[];
			var all_columns = _DatabaseExplorer.SelectedTable.Column;
			if (columns == null) {
				return;
			}
			
			if (_MiValidateColumnsOnPaste.IsChecked) {
				foreach (var column in columns) {
					var found_column = all_columns.FirstOrDefault(col => col.Name.ToLower() == column.Name.ToLower());

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
				_DatabaseExplorer.SelectedTable.Column.Insert(_dagColumnDefinitions.SelectedIndex + 1, columns[i]);
			}
			
		}



		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			Exit(e);
		}

		private void Exit(System.ComponentModel.CancelEventArgs e) {
			if (_DatabaseExplorer.CloseAllDatabases() == false) {
				e.Cancel = true;
			}
		}

		bool auto = false;

		private void ConfigurationChangedEvent(object sender, TextChangedEventArgs e) {
			string test = auto.ToString();
		}

		private void Configuration_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
			auto = true;
		}

		private void _TxtConfigNamespace_TargetUpdated(object sender, DataTransferEventArgs e) {
			auto = false;
		}




	}

}
