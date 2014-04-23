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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using DtxModel.Ddl;
using System.Xml.Serialization;
using System.Threading;
using System.Collections.ObjectModel;

namespace DtxModeler.Xaml {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private Database ddl;

		private DtxModel.Ddl.Table selected_table;

		public MainWindow() {
			InitializeComponent();
		}

		public void openDdl() {
			var dialog = new OpenFileDialog() {
				Filter = "Ddl files (*.ddl)|*.ddl",
				Multiselect = false
			};

			var status = dialog.ShowDialog();
			var serializer = new XmlSerializer(typeof(Database));

			if (status.HasValue == false || status.Value == false) {
				return;
			}

			ThreadPool.QueueUserWorkItem(o => { 
				using (var ddl_stream = dialog.OpenFile()) {
					try {
						ddl = (Database)serializer.Deserialize(ddl_stream);
					} catch (Exception) {
						this.Dispatcher.BeginInvoke(new Action(() => {
							MessageBox.Show("Unable to load selected Ddl file.");
						}), null);
						return;
					}
				}

				this.Dispatcher.BeginInvoke(new Action(refreshDdl), null);
			});

			
		}

		private void refreshDdl() {
			_treDatabaseLayout.ItemsSource = new ObservableCollection<DtxModel.Ddl.Table>(ddl.Table);
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e) {
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			//e.
		}

		private void _miOpen_Click(object sender, RoutedEventArgs e) {
			openDdl();
		}

		private void _treDatabaseLayout_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
			if (e.NewValue is DtxModel.Ddl.Table) {
				selected_table = (DtxModel.Ddl.Table)e.NewValue;
			}
		}

	}
}
