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
			ColumnNetType.ItemsSource = type_transformer.NetTypes();
		}

		private Column GetSelectedColumn() {
			return _dagColumnDefinitions.SelectedItem as Column;
			
		}

		private Column[] GetSelectedColumns() {
			return _dagColumnDefinitions.SelectedItems.Cast<Column>().ToArray();
		}



		private void New_Click(object sender, RoutedEventArgs e) {
			_DatabaseExplorer.OpenDdl(true);
		}

		private void Open_Click(object sender, RoutedEventArgs e) {
			_DatabaseExplorer.OpenDdl();
		}

		private void Exit_Click(object sender, RoutedEventArgs e) {
			Exit(new System.ComponentModel.CancelEventArgs());
		}

		private void Save_Click(object sender, RoutedEventArgs e) {
			_DatabaseExplorer.SaveCurrent(false);
		}

		private void SaveAs_Click(object sender, RoutedEventArgs e) {
			_DatabaseExplorer.SaveCurrent(true);
		}

		void Column_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			Column column = sender as Column;
			_DatabaseExplorer.SelectedDatabase._Modified = true;

			// Temporarily remove this event so that we do not get stuck with a stack overflow.
			column.PropertyChanged -= Column_PropertyChanged;


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
			column.PropertyChanged += Column_PropertyChanged;
		}


		private void _DatabaseExplorer_ChangedSelection(object sender, ExplorerSelectionChangedEventArgs e) {
			_dagColumnDefinitions.ItemsSource = null;
			_TxtTableDescription.IsEnabled = _TxtColumnDescription.IsEnabled = false;
			_TxtTableDescription.Text = _TxtColumnDescription.Text = "";

			if (e.SelectionType == ExplorerSelection.TableItem) {
				if (e.Table.Column == null) {
					e.Table.Column = new Column[0];
				}

				// Setup our validation on the data.
				if (e.Table._ObservableColumns == null) {
					bool initial_add = true;
					e.Table._ObservableColumns = new ObservableCollection<Column>();

					e.Table._ObservableColumns.CollectionChanged += ((coll_sender, coll_e) => {
						
						if (coll_e.Action == NotifyCollectionChangedAction.Add) {

							// Don't change the inital array if we are still adding to it.
							if (initial_add != true) {
								// Keep the observable collection in sync with the XML array.
								e.Table.Column = ((ObservableCollection<Column>)coll_sender).ToArray();
								_DatabaseExplorer.SelectedDatabase._Modified = true;
							}

							// If we add a new column, add a new property changed event to it.
							foreach (Column column in coll_e.NewItems) {
								column.PropertyChanged += Column_PropertyChanged;
							}
						}


					});

					// Add each column to the observable collection.
					foreach (var column in e.Table.Column) {
						e.Table._ObservableColumns.Add(column);
					}

					initial_add = false;
				}

				_dagColumnDefinitions.ItemsSource = e.Table._ObservableColumns;
				_tabTable.IsSelected = true;

				_TxtTableDescription.IsEnabled = true;
				_TxtTableDescription.Text = e.Table.Description;

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

		private void _TxtTableDescription_TextChanged(object sender, TextChangedEventArgs e) {
			if (_TxtTableDescription.IsEnabled) {
				_DatabaseExplorer.SelectedTable.Description = _TxtTableDescription.Text;
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
				_DatabaseExplorer.SelectedTable._ObservableColumns.Remove(column);
			}

		}

		private void _CmiMoveColumnUp_Click(object sender, RoutedEventArgs e) {
			var columns = GetSelectedColumns();
			var all_columns = _DatabaseExplorer.SelectedTable._ObservableColumns;

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
			var all_columns = _DatabaseExplorer.SelectedTable._ObservableColumns;

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
			var all_columns = _DatabaseExplorer.SelectedTable._ObservableColumns;
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
				_DatabaseExplorer.SelectedTable._ObservableColumns.Insert(_dagColumnDefinitions.SelectedIndex + 1, columns[i]);
			}
			
		}



		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			Exit(e);
		}

		private void Exit(System.ComponentModel.CancelEventArgs e) {
			foreach (var database in _DatabaseExplorer.ChangedDatabases) {
				


			}
		}


	}

}
