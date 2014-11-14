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
			DataContext = this;
		}



		public Table SelectedTable {
			get { return (Table)GetValue(SelectedTableProperty); }
			set { SetValue(SelectedTableProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedTable.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedTableProperty =
			DependencyProperty.Register("SelectedTable", typeof(Table), typeof(IndexTabControl), null);


		/*
		public Table SelectedTable {
			get {
				return (Table)GetValue(SelectedTableProperty);
			}
			set {
				SetValue(SelectedTableProperty, value);
			}
		}

		public static readonly DependencyProperty SelectedTableProperty =
			DependencyProperty.Register(
				"SelectedTable",
				typeof(Table),
				typeof(IndexTabControl),
				new PropertyMetadata(default(Table), OnItemsPropertyChanged));

		private static void OnItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			string run = "";
			//throw new NotImplementedException();
		}
		*/
		public Index SelectedIndex { get; set; }

		public int MyProperty { get; set; }

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
