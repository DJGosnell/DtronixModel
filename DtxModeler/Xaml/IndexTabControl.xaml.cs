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

		public Table SelectedTable {
			get { return (Table)GetValue(SelectedTableProperty); }
			set { SetValue(SelectedTableProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedTable2.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedTableProperty =
			DependencyProperty.Register("SelectedTable", typeof(Table), typeof(IndexTabControl));



		public void Refresh() {
			var idx = new Index() {
				Description = "Test Index Description",
				Name = "Test Index " + SelectedTable.Index.Count.ToString()
			};

			idx.IndexColumn.Add(new IndexColumn() {
				Direction = Order.Ascending,
				Name = "Column 1"
			});
			idx.IndexColumn.Add(new IndexColumn() {
				Direction = Order.Ascending,
				Name = "Column 2"
			});

			SelectedTable.Index.Add(idx);
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

	}
}
