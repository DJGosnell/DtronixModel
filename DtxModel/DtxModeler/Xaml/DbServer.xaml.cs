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

		private void Test_Click(object sender, RoutedEventArgs e) {

		}

		private void Save_Click(object sender, RoutedEventArgs e) {

		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {

		}

		private void _cmbProvider_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			string value = ((ComboBoxItem)_cmbProvider.SelectedValue).Content.ToString().ToLower();
			if (value == "mysql") {
				_txtPassword.IsEnabled = _txtPassword.IsEnabled = true;
			} else if (value == "sqlite") {
				_txtPassword.IsEnabled = true;
				_txtPassword.IsEnabled = false;
			}
		}
	}
}
