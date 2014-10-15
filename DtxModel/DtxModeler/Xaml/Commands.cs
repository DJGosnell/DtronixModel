using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DtxModeler.Xaml {
	public static class Commands {

		public static readonly RoutedUICommand NewDatabaseItem = new RoutedUICommand("New", "NewDatabaseItem", typeof(Commands));
		public static readonly RoutedUICommand CloseDatabase = new RoutedUICommand("Close Database", "CloseDatabase", typeof(Commands));
		public static readonly RoutedUICommand OpenDatabaseDirectory = new RoutedUICommand("Open Containing Directory", "OpenDatabaseDirectory", typeof(Commands));
		public static readonly RoutedUICommand RenameDatabaseItem = new RoutedUICommand("Rename", "RenameDatabaseItem", typeof(Commands));
		public static readonly RoutedUICommand CreateDatabase = new RoutedUICommand("Create Database", "CreateDatabase", typeof(Commands));
		//public static readonly RoutedUICommand SomeOtherAction = new RoutedUICommand("Some other action", "SomeOtherAction", typeof(Window1));
		//public static readonly RoutedUICommand MoreDeeds = new RoutedUICommand("More deeds", "MoreDeeeds", typeof(Window1));
	}
}
