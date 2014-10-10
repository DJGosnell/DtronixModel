using DtxModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for AssociationWindow.xaml
	/// </summary>
	public partial class AssociationWindow : Window {
		private Database database;
		private Association association;

		public AssociationWindow(Database database, Association association) {
			this.database = database;
			this.association = association;

			if (association == null) {
				association = new Association();
			}

			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			_CmbTable1.ItemsSource = database.Table;
			_CmbTable1.SelectedItem = database.Table.FirstOrDefault(table => table.Name == association.Table1);

			if (_CmbTable1.SelectedItem != null) {
				_CmbTable1Column.SelectedItem = (_CmbTable1.SelectedItem as Table).Column.FirstOrDefault(column => column.Name == association.Table1Column);
			}
			
			

			//._CmbTable1Cardinality.ItemsSource = Enum.GetValues(typeof(Cardinality)).Cast<Cardinality>();
		}

		private void _CmbTable1_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			var table = _CmbTable1.SelectedItem as Table;
			_CmbTable1Column.ItemsSource = table.Column;
		}

		private void _CmbTable1Column_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			// If the text is empty, then set a new default name.
			if (_TxtTable1Name.Text.Trim() == "") {
				_TxtTable1Name.Text = _CmbTable1.SelectedValue.ToString();
			}
		}


	}
}
