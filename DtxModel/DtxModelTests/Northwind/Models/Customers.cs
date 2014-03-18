using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DtxModel;
using System.Data.Common;
using System.Reflection;

namespace DtxModelTests.Northwind.Models {

	[TableAttribute(Name = "Customers")]
	[ColumnAttribute(Name = "rowid", Storage = "_rowid", IsDbGenerated = true)]
	[ColumnAttribute(Name = "CustomerID", Storage = "_CustomerID")]
	[ColumnAttribute(Name = "CompanyName", Storage = "_CompanyName")]
	[ColumnAttribute(Name = "ContactName", Storage = "_ContactName")]
	[ColumnAttribute(Name = "Address", Storage = "_Address")]
	[ColumnAttribute(Name = "City", Storage = "_City")]
	[ColumnAttribute(Name = "Region", Storage = "_Region")]
	[ColumnAttribute(Name = "PostalCode", Storage = "_PostalCode")]
	[ColumnAttribute(Name = "Fax", Storage = "_Fax")]
	[ColumnAttribute(Name = "Phone", Storage = "_Phone")]
	[ColumnAttribute(Name = "Country", Storage = "_Country")]
	class Customers : Model {

		private static Dictionary<string, FieldInfo> column_fields;

		private long _rowid;

		public long rowid {
			get { return _rowid; }
		}

		private string _CustomerID;

		public string CustomerID {
			get { return _CustomerID; }
			set {
				_CustomerID = value;
				if (changed_columns.Contains("CustomerID") == false) {
					changed_columns.Add("CustomerID");
				}
			}
		}

		private string _CompanyName;

		public string CompanyName {
			get { return _CompanyName; }
			set {
				_CompanyName = value;
				if (changed_columns.Contains("CompanyName") == false) {
					changed_columns.Add("CompanyName");
				}
			}
		}

		private string _ContactName;

		public string ContactName {
			get { return _ContactName; }
			set {
				_ContactName = value;
				if (changed_columns.Contains("ContactName") == false) {
					changed_columns.Add("ContactName");
				}
			}
		}

		private string _Address;

		public string Address {
			get { return _Address; }
			set {
				_Address = value;
				if (changed_columns.Contains("Address") == false) {
					changed_columns.Add("Address");
				}
			}
		}

		private string _City;

		public string City {
			get { return _City; }
			set {
				_City = value;
				if (changed_columns.Contains("City") == false) {
					changed_columns.Add("City");
				}
			}
		}

		private string _Region;

		public string Region {
			get { return _Region; }
			set {
				_Region = value;
				if (changed_columns.Contains("Region") == false) {
					changed_columns.Add("Region");
				}
			}
		}

		private string _PostalCode;

		public string PostalCode {
			get { return _PostalCode; }
			set {
				_PostalCode = value;
				if (changed_columns.Contains("PostalCode") == false) {
					changed_columns.Add("PostalCode");
				}
			}
		}

		private string _Country;

		public string Country {
			get { return _Country; }
			set {
				_Country = value;
				if (changed_columns.Contains("Country") == false) {
					changed_columns.Add("Country");
				}
			}
		}

		private string _Phone;

		public string Phone {
			get { return _Phone; }
			set {
				_Phone = value;
				if (changed_columns.Contains("Phone") == false) {
					changed_columns.Add("Phone");
				}
			}
		}

		private string _Fax;

		public string Fax {
			get { return _Fax; }
			set {
				_Fax = value;
				if (changed_columns.Contains("Fax") == false) {
					changed_columns.Add("Fax");
				}
			}
		}

		public Customers() : this(null, null) { }

		public Customers(DbDataReader reader, DbConnection connection) {
			if (column_fields == null) {
				column_fields = getColumnFields<Customers>();
			}

			read(reader, connection);
		}


		public override void read(DbDataReader reader, DbConnection connection) {
			if (reader == null) {
				return;
			}
			string column_name;
			int length = reader.FieldCount;

			for (int i = 0; i < length; i++) {
				column_name = reader.GetName(i);

				if(column_fields.ContainsKey(column_name) == false){ 
					continue;
				}

				var val = reader.GetValue(i);

				// If the value is not null, set the field to the value.  If the value is DbNull, do nothing and use the default values.
				if (Convert.IsDBNull(val) == false) {
					column_fields[column_name].SetValue(this, val);
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var values = new Dictionary<string, object>();
			foreach (var column in changed_columns) {
				values.Add(column, column_fields[column].GetValue(this));
			}

			return values;
		}


		public override object[] getAllValues() {
			var values = new object[column_fields.Count];
			var i = 0;

			foreach (var fields in column_fields) {
				values[i++] = fields.Value.GetValue(this);
			}

			return values;
		}

		public override string[] getColumns() {
			return column_fields.Keys.ToArray();
		}

	}
}
