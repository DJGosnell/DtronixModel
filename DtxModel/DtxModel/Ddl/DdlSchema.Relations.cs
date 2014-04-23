using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DtxModel.Ddl {
	public partial class Table {

		[XmlIgnore]
		private ObservableCollection<Column> _ColumnCollection;
		[XmlIgnore]
		public ObservableCollection<Column> ColumnCollection {
			get {
				if (_ColumnCollection == null) {
					_ColumnCollection = new ObservableCollection<Column>(this.columnField);
				}
				return _ColumnCollection;

			}
		}
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

