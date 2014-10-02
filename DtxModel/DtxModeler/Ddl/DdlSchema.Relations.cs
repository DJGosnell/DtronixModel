using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace DtxModeler.Ddl {
	public partial class Table {
		[XmlIgnore]
		public ObservableCollection<Column> _ObservableColumns;

	}

	public partial class Database {
		[XmlIgnore]
		public bool _Modified;

		[XmlIgnore]
		public string _FileLocation;

		[XmlIgnore]
		public TreeViewItem _TreeRoot;

	}



	public partial class Association {

		[XmlIgnore]
		public Association ChildAssociation;

		[XmlIgnore]
		public Association ParentAssociation;

		[XmlIgnore]
		public Column OtherKeyColumn;

		[XmlIgnore]
		public Column ThisKeyColumn;

		[XmlIgnore]
		public Table Table;
	}

	public partial class Column {

		/// <remarks/>
		[XmlIgnore]
		public Table Table;
	}

	public partial class Index {

		/// <remarks/>
		[XmlIgnore]
		public Table Table;
	}
}

