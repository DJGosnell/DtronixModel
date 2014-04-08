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


	public class Conference {
		public Conference(string name) {
			Name = name;
			Teams = new ObservableCollection<Team>();
		}

		public string Name { get; private set; }
		public ObservableCollection<Team> Teams { get; private set; }
	}

	public class Team {
		public Team(string name) {
			Name = name;
			Players = new ObservableCollection<string>();
		}

		public string Name { get; private set; }
		public ObservableCollection<string> Players { get; private set; }
	}


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private Database ddl;

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


			var western = new Conference("Western") {
				Teams =
                {
                    new Team("Club Deportivo Chivas USA"),
                    new Team("Colorado Rapids"),
                    new Team("FC Dallas"),
                    new Team("Houston Dynamo"),
                    new Team("Los Angeles Galaxy"),
                    new Team("Real Salt Lake"),
                    new Team("San Jose Earthquakes"),
                    new Team("Seattle Sounders FC") { Players = { "Osvaldo Alonso", "Evan Brown" }},
                    new Team("Portland 2011"),
                    new Team("Vancouver 2011")
                }
			};

			var eastern = new Conference("Eastern") {
				Teams =
                {
                    new Team("Chicago Fire"),
                    new Team("Columbus Crew"),
                    new Team("D.C. United"),
                    new Team("Kansas City Wizards"),
                    new Team("New York Red Bulls"),
                    new Team("New England Revolution"),
                    new Team("Toronto FC"),
                    new Team("Philadelphia Union 2010")
                }
			};

			var league = new Collection<Conference>() { western, eastern };


			_treDatabaseLayout.DataContext = new {
				//WesternConference = western,
				//EasternConference = eastern,
				League = league,
				DataB = ddl.Table
			};

			//_treDatabaseLayout.DataContext = ddl;
			/*var root = new TreeViewItem() {
				Header = ddl.Name
			};

			foreach (var table in ddl.Table) {
				var table_item = new TreeViewItem() {
					Header = table.Name,
					
				};

				root.Items.Add(table_item);
			}

			_treDatabaseLayout.Items.Clear();
			_treDatabaseLayout.Items.Add(root);*/
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e) {
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			//e.
		}

		private void _miOpen_Click(object sender, RoutedEventArgs e) {
			openDdl();
		}

	}
}
