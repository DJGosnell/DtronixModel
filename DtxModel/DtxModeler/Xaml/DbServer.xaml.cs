using DtxModeler.Generator;
using DtxModeler.Generator.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for DbServer.xaml
	/// </summary>
	public partial class DbServer : Window {
		private enum DbProvider : int {
			Sqlite = 0,
			MySQL = 1
		}


		public DbServer() {
			InitializeComponent();
		}

		private void ServerBrowse_Click(object sender, RoutedEventArgs e) {
			var server_file = new Microsoft.Win32.OpenFileDialog();
			server_file.CheckFileExists = false;
			server_file.Multiselect = false;

			var result = server_file.ShowDialog();
			if (result.HasValue == false || result == false) {
				return;
			}

			_txtServer.Text = server_file.FileName;
		}

		private string generateConnectionString() {
			var sb = new StringBuilder();

			return sb.ToString();


		}

		private void Test_Click(object sender, RoutedEventArgs e) {
			var connection_string = generateConnectionString();
			DdlGenerator generator = null;

			switch ((DbProvider)_cmbProvider.SelectedIndex) {
				case DbProvider.Sqlite:
					generator = new SqliteDdlGenerator(connection_string);
					break;
				case DbProvider.MySQL:

					break;
				default:
					break;
			}

			var ddl = generator.generateDdl();


		}

		private void Save_Click(object sender, RoutedEventArgs e) {

		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {

		}

		private void _cmbProvider_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			switch ((DbProvider)_cmbProvider.SelectedIndex) {
				case DbProvider.Sqlite:
					_txtPassword.IsEnabled = true;
					_txtPassword.IsEnabled = false;
					break;
				case DbProvider.MySQL:
					_txtPassword.IsEnabled = _txtPassword.IsEnabled = true;
					break;
				default:
					break;
			}
		}
	}
}
