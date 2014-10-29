using DtxModeler.Ddl;
using DtxModeler.Generator;
using DtxModeler.Generator.Sqlite;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for DatabaseServer.xaml
	/// </summary>
	public partial class DatabaseServer : Window {
		byte[] sqlite_header = new byte[] { 0x53, 0x51, 0x4c, 0x69, 0x74, 0x65, 0x20, 0x66, 0x6f, 0x72, 0x6d, 0x61, 0x74, 0x20, 0x33, 0x00 };
		private bool sqlite_add_rowid;

		public string ConnectionString {
			get {
				var sb = new StringBuilder();

				switch ((DbProvider)_CmbProvider.SelectedItem) {
					case DbProvider.Sqlite:
						sb.Append("Data Source=\"").Append(_TxtServer.Text).Append("\";");
						sb.Append("Version=3;");
						break;
					case DbProvider.MySQL:

						break;
					default:
						break;
				}

				return sb.ToString();
			}
		}

		/// <summary>
		/// Returns an unopened DB connection with the proper settings for this database.
		/// </summary>
		public DbConnection Connection {
			get {
				switch (Provider) {
					case DbProvider.Sqlite:
						return new SQLiteConnection(ConnectionString);
					default:
						throw new NotImplementedException("MySQL servers not implimented yet.");
				}

			}

		}

		public DbProvider Provider {
			get {
				return (DbProvider)_CmbProvider.SelectedItem;
			}
		}


		public Database Database {
			get {
				DdlGenerator generator = null;
				string name = null;

				switch (Provider) {
					case DbProvider.Sqlite:
						generator = new SqliteDdlGenerator(ConnectionString);
						name = Path.GetFileName(_TxtServer.Text);
						break;
					default:
						throw new NotImplementedException("MySQL servers not implimented yet.");
				}

				var database = generator.generateDdl();
				database.Name = name;

				if (sqlite_add_rowid) {
					foreach (var table in database.Table) {
						table.Column.Insert(0, new Column() {
							Name = "rowid",
							IsAutoIncrement = true,
							IsReadOnly = true,
							Nullable = false,
							Description = "Auto generated SQLite rowid column.",
							IsPrimaryKey = true,
							DbType = "INTEGER",
							NetType = NetTypes.Int64,
						});
					}
				}

				database.Initialize();
				

				return database;
			}
		}


		public DatabaseServer() {
			InitializeComponent();

			_CmbProvider.ItemsSource = Enum.GetValues(typeof(DbProvider)).Cast<DbProvider>();
		}

		private void ServerBrowse_Click(object sender, RoutedEventArgs e) {
			var server_file = new OpenFileDialog();
			server_file.CheckFileExists = false;
			server_file.Multiselect = false;

			if (server_file.ShowDialog() == true) {
				_TxtServer.Text = server_file.FileName;
			}


		}

		private void Test_Click(object sender, RoutedEventArgs e) {
			switch (Provider) {
				case DbProvider.Sqlite:

					if (File.Exists(_TxtServer.Text) == false) {
						MessageBox.Show("Sqlite file does not exist.");
						return;
					}

					try {
						using (var stream = File.Open(_TxtServer.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
							for (int i = 0; i < sqlite_header.Length; i++) {
								if (stream.ReadByte() != sqlite_header[i]) {
									MessageBox.Show("Specified file is not a SQLite database file.");
									return;
								}
							}
						}

						using (var connection = this.Connection) {
							connection.Open();
							MessageBox.Show("Successfully Verified database.");
						}
					} catch (Exception ex) {
						MessageBox.Show("Could not open database specified.\r\n" + ex.ToString());
					}

					break;
				case DbProvider.MySQL:
					break;
				default:
					break;
			}


		}
		private void Cancel_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = false;
		}

		private void _cmbProvider_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			switch ((DbProvider)_CmbProvider.SelectedIndex) {
				case DbProvider.Sqlite:
					//_txtPassword.IsEnabled = true;
					_txtPassword.IsEnabled = false;
					break;
				case DbProvider.MySQL:
					_txtPassword.IsEnabled = _txtPassword.IsEnabled = true;
					break;
				default:
					break;
			}
		}

		private void Open_Click(object sender, RoutedEventArgs e) {
			if ((DbProvider)_CmbProvider.SelectedValue == DbProvider.Sqlite) {
				if (MessageBox.Show("Do you want to automatically add the 'rowid' column to all the tables?", "SQLite Import", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
					sqlite_add_rowid = true;
				}
			};
			this.DialogResult = true;
		}
	}
}
