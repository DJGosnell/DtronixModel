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

		public Association Association {
			get { return association; }
		}
		

		public AssociationWindow(Database database, Association association) {
			this.database = database;
			this.association = association;

			if (association == null) {
				association = new Association();
			}

			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			_CmbTable2.ItemsSource = _CmbTable1.ItemsSource = database.Table;
			_CmbTable1.SelectedItem = database.Table.FirstOrDefault(table => table.Name == association.Table1);
			_CmbTable2.SelectedItem = database.Table.FirstOrDefault(table => table.Name == association.Table2);
			_CmbCardinality.SelectedIndex = 0;

			if (_CmbTable1.SelectedItem != null) {
				_CmbTable1Column.SelectedItem = (_CmbTable1.SelectedItem as Table).Column.FirstOrDefault(column => column.Name == association.Table1Column);
				_CmbCardinality.SelectedIndex = 1;
			}

			if (_CmbTable2.SelectedItem != null) {
				_CmbTable2Column.SelectedItem = (_CmbTable2.SelectedItem as Table).Column.FirstOrDefault(column => column.Name == association.Table2Column);
			}
		}

		private void _CmbTable1_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			_TxtTable1Name.Text = _CmbTable1.SelectedValue.ToString();

			if(_CmbTable1.SelectedItem != null) {
				_CmbTable1Column.ItemsSource = (_CmbTable1.SelectedItem as Table).Column;
			}
		}


		private void _CmbTable2_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			_TxtTable2Name.Text = _CmbTable2.SelectedValue.ToString();

			if (_CmbTable2.SelectedItem != null) {
				_CmbTable2Column.ItemsSource = (_CmbTable2.SelectedItem as Table).Column;
			}
		}

		private void _BtnOk_Click(object sender, RoutedEventArgs e) {
			if(_CmbTable1.SelectedItem == null || _CmbTable2.SelectedItem == null ||
				_CmbTable1Column.SelectedItem == null || _CmbTable2Column.SelectedItem == null ||
				string.IsNullOrWhiteSpace(_TxtTable1Name.Text) || string.IsNullOrWhiteSpace(_TxtTable2Name.Text) ||
				_CmbCardinality.SelectedItem == null) {
					MessageBox.Show("Association form not completed.");
					return;
			}

			association.Table1 = (_CmbTable1.SelectedItem as Table).Name;
			association.Table2 = (_CmbTable2.SelectedItem as Table).Name;

			association.Table1Column = (_CmbTable1Column.SelectedItem as Column).Name;
			association.Table2Column = (_CmbTable2Column.SelectedItem as Column).Name;

			association.Table1Name = _TxtTable1Name.Text;
			association.Table2Name = _TxtTable2Name.Text;

			switch (_CmbCardinality.SelectedIndex) {
				case 0:
					association.Table1Cardinality = association.Table2Cardinality = Cardinality.One;
					break;
				case 1:
					association.Table1Cardinality = Cardinality.Many;
					association.Table2Cardinality = Cardinality.One;
					break;
				case 2:
					association.Table1Cardinality = Cardinality.One;
					association.Table2Cardinality = Cardinality.Many;
					break;
				case 3:
					association.Table1Cardinality = association.Table2Cardinality = Cardinality.Many;
					break;
			}


			this.DialogResult = true;
		}

		private void _BtnCancel_Click(object sender, RoutedEventArgs e) {
			this.DialogResult = false;
		}



	}
}
