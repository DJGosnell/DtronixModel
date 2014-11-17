using DtxModeler.Ddl;
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
using System.Windows.Shapes;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for IndexTabControl.xaml
	/// </summary>
	public partial class IndexTabControl : UserControl {
		public IndexTabControl() {
			InitializeComponent();
			_ColCmbSortDirection.ItemsSource = Enum.GetValues(typeof(Order)).Cast<Order>();
		}



		public string[] SelectedTableColumns {
			get { return (string[])GetValue(SelectedTableColumnsProperty); }
			set { SetValue(SelectedTableColumnsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedTableColumns.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedTableColumnsProperty =
			DependencyProperty.Register("SelectedTableColumns", typeof(string[]), typeof(IndexTabControl));



		public Table SelectedTable {
			get { return (Table)GetValue(SelectedTableProperty); }
			set { SetValue(SelectedTableProperty, value); }
		}
		

		// Using a DependencyProperty as the backing store for SelectedTable2.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedTableProperty =
			DependencyProperty.Register("SelectedTable", typeof(Table), typeof(IndexTabControl), new PropertyMetadata(SelectedTablePropertyChanged));

		public static void SelectedTablePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((IndexTabControl)d).UpdateColumnList();

			
		}

		private void UpdateColumnList() {
			if (SelectedTable == null) {
				SelectedTableColumns = null;
				return;
			}

			var columns = SelectedTable.Column;
			string[] column_names = new string[columns.Count];

			for (int i = 0; i < columns.Count; i++) {
				column_names[i] = columns[i].Name;
			}

			SelectedTableColumns = column_names;

			var s = this.col;
		}

		private void _LstIndexes_New(object sender, ExecutedRoutedEventArgs e) {
			SelectedTable.Index.Add(new Index() {
				Name = "ix_" + SelectedTable.Name + (SelectedTable.Index.Count + 1).ToString() 
			});
		}

		private void _LstIndexes_NewCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = SelectedTable != null;
		}

		private void _LstIndexes_Delete(object sender, ExecutedRoutedEventArgs e) {
			SelectedTable.Index.Remove(_LstIndexes.SelectedItem as Index);
		}

		private void _LstIndexes_DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = _LstIndexes.SelectedItem != null;
		}

		private void _LstIndexes_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {
			_LstIndexes.Focus();
		}

		private void _LstIndexes_SelectionChanged(object sender, SelectionChangedEventArgs e) {

		}

	}
}
