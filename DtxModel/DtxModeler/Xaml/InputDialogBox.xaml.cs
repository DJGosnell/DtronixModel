using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	/// Interaction logic for InputDialogBox.xaml
	/// </summary>
	public partial class InputDialogBox : Window {



		public string Description {
			get { return _LblDescription.Text; }
			set { _LblDescription.Text = value; }
		}

		public string Value {
			get { return _TxtValue.Text.Trim(); }
			set { _TxtValue.Text = value; }
		}

		

		public InputDialogBox() {
			InitializeComponent();
			_TxtValue.Focus();
		}

		private void _BtnOk_Click(object sender, RoutedEventArgs e) {
			if (_TxtValue.Text.Trim() == "") {
				return;
			}
			DialogResult = true;
			this.Close();
		}

		private void _BtnCancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
			this.Close();
		}
	}
}
