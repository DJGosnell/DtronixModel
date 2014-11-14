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
		}


		private Table _SelectedTable = null;

		public Table SelectedTable {
			get { return (Table)GetValue(SelectedTableProperty); }
			set { SetValue(SelectedTableProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedTable2.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedTableProperty =
			DependencyProperty.Register("SelectedTable", typeof(Table), typeof(IndexTabControl), new PropertyMetadata(SelectedTable_PropertyChanged));

		private static void SelectedTable_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((IndexTabControl)d).Refresh();
		}

		public void Refresh() {
			_SelectedTable = SelectedTable;
			if (_SelectedTable == null) {
				return;
			}
			var idx = new Index() {
				Description = "Test Index Description",
				Name = "Test Index " + _SelectedTable.Index.Count.ToString()
			};

			idx.IndexColumn.Add(new IndexColumn() {
				Direction = Order.Ascending,
				Name = "Column 1"
			});
			idx.IndexColumn.Add(new IndexColumn() {
				Direction = Order.Ascending,
				Name = "Column 2"
			});

			_SelectedTable.Index.Add(idx);
		}

		private void _LstIndexes_New(object sender, ExecutedRoutedEventArgs e) {

		}

		private void _LstIndexes_NewCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = _LstIndexes.SelectedItem != null;
		}

		private void _LstIndexes_Delete(object sender, ExecutedRoutedEventArgs e) {

		}

		private void _LstIndexes_DeleteCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = _LstIndexes.SelectedItem != null;
		}

	}
}
