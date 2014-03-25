using System;
using System.Data.Common;
using System.Collections.Generic;
using DtxModel;

namespace DtxModelTests.Northwind {

	public class NorthwindContext : Context {
		private static Func<DbConnection> _default_connection = null;

		/// <summary>
		/// Set a default constructor to allow use of parameterless context calling.
		/// </summary>
		public static Func<DbConnection> DefaultConnection {
			get { return _default_connection; }
			set { _default_connection = value; }
		}

		private Table<Customers> _Customers;

		public Table<Customers> Customers {
			get {
				if(_Customers == null) {
					_Customers = new Table<Customers>(connection);
				}

				return _Customers;}
		}

		private Table<Categories> _Categories;

		public Table<Categories> Categories {
			get {
				if(_Categories == null) {
					_Categories = new Table<Categories>(connection);
				}

				return _Categories;}
		}

		/// <summary>
		/// Create a new context of this database's type.  Can only be used if a default connection is specified.
		/// </summary>
		public NorthwindContext() : base(_default_connection) { }

		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public NorthwindContext(DbConnection connection) : base(connection) { }
	}

	[TableAttribute(Name = "Customers")]
	public class Customers : Model {
		private System.Int64 _rowid;
		public System.Int64 rowid {
			get { return _rowid; }
		}

		private bool _CustomerIDChanged = false;
		private System.String _CustomerID;
		public System.String CustomerID {
			get { return _CustomerID; }
			set {
				_CustomerID = value;
				_CustomerIDChanged = true;
			}
		}

		private bool _CompanyNameChanged = false;
		private System.String _CompanyName;
		public System.String CompanyName {
			get { return _CompanyName; }
			set {
				_CompanyName = value;
				_CompanyNameChanged = true;
			}
		}

		private bool _ContactNameChanged = false;
		private System.String _ContactName;
		public System.String ContactName {
			get { return _ContactName; }
			set {
				_ContactName = value;
				_ContactNameChanged = true;
			}
		}

		private bool _ContactTitleChanged = false;
		private System.String _ContactTitle;
		public System.String ContactTitle {
			get { return _ContactTitle; }
			set {
				_ContactTitle = value;
				_ContactTitleChanged = true;
			}
		}

		private bool _AddressChanged = false;
		private System.String _Address;
		public System.String Address {
			get { return _Address; }
			set {
				_Address = value;
				_AddressChanged = true;
			}
		}

		private bool _CityChanged = false;
		private System.String _City;
		public System.String City {
			get { return _City; }
			set {
				_City = value;
				_CityChanged = true;
			}
		}

		private bool _RegionChanged = false;
		private System.String _Region;
		public System.String Region {
			get { return _Region; }
			set {
				_Region = value;
				_RegionChanged = true;
			}
		}

		private bool _PostalCodeChanged = false;
		private System.String _PostalCode;
		public System.String PostalCode {
			get { return _PostalCode; }
			set {
				_PostalCode = value;
				_PostalCodeChanged = true;
			}
		}

		private bool _CountryChanged = false;
		private System.String _Country;
		public System.String Country {
			get { return _Country; }
			set {
				_Country = value;
				_CountryChanged = true;
			}
		}

		private bool _phoneChanged = false;
		private System.String _phone;
		public System.String phone {
			get { return _phone; }
			set {
				_phone = value;
				_phoneChanged = true;
			}
		}

		private bool _FaxChanged = false;
		private System.String _Fax;
		public System.String Fax {
			get { return _Fax; }
			set {
				_Fax = value;
				_FaxChanged = true;
			}
		}

		private bool _Categories_rowidChanged = false;
		private System.Int64 _Categories_rowid;
		public System.Int64 Categories_rowid {
			get { return _Categories_rowid; }
			set {
				_Categories_rowid = value;
				_Categories_rowidChanged = true;
			}
		}

		public Customers() : this(null, null) { }

		public Customers(DbDataReader reader, DbConnection connection) {
			read(reader, connection);
		}

		public override void read(DbDataReader reader, DbConnection connection) {
			this.connection = connection;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (System.Int64)reader.GetValue(i); break;
					case "CustomerID": _CustomerID = reader.GetValue(i) as System.String; break;
					case "CompanyName": _CompanyName = reader.GetValue(i) as System.String; break;
					case "ContactName": _ContactName = reader.GetValue(i) as System.String; break;
					case "ContactTitle": _ContactTitle = reader.GetValue(i) as System.String; break;
					case "Address": _Address = reader.GetValue(i) as System.String; break;
					case "City": _City = reader.GetValue(i) as System.String; break;
					case "Region": _Region = reader.GetValue(i) as System.String; break;
					case "PostalCode": _PostalCode = reader.GetValue(i) as System.String; break;
					case "Country": _Country = reader.GetValue(i) as System.String; break;
					case "phone": _phone = reader.GetValue(i) as System.String; break;
					case "Fax": _Fax = reader.GetValue(i) as System.String; break;
					case "Categories_rowid": _Categories_rowid = (System.Int64)reader.GetValue(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_CustomerIDChanged)
				changed.Add("CustomerID", _CustomerID);
			if (_CompanyNameChanged)
				changed.Add("CompanyName", _CompanyName);
			if (_ContactNameChanged)
				changed.Add("ContactName", _ContactName);
			if (_ContactTitleChanged)
				changed.Add("ContactTitle", _ContactTitle);
			if (_AddressChanged)
				changed.Add("Address", _Address);
			if (_CityChanged)
				changed.Add("City", _City);
			if (_RegionChanged)
				changed.Add("Region", _Region);
			if (_PostalCodeChanged)
				changed.Add("PostalCode", _PostalCode);
			if (_CountryChanged)
				changed.Add("Country", _Country);
			if (_phoneChanged)
				changed.Add("phone", _phone);
			if (_FaxChanged)
				changed.Add("Fax", _Fax);
			if (_Categories_rowidChanged)
				changed.Add("Categories_rowid", _Categories_rowid);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_CustomerID,
				_CompanyName,
				_ContactName,
				_ContactTitle,
				_Address,
				_City,
				_Region,
				_PostalCode,
				_Country,
				_phone,
				_Fax,
				_Categories_rowid,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"CustomerID",
				"CompanyName",
				"ContactName",
				"ContactTitle",
				"Address",
				"City",
				"Region",
				"PostalCode",
				"Country",
				"phone",
				"Fax",
				"Categories_rowid",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override System.Int64 getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "Categories")]
	public class Categories : Model {
		private System.Int64 _rowid;
		public System.Int64 rowid {
			get { return _rowid; }
		}

		private bool _CategoryIDChanged = false;
		private System.Int32 _CategoryID;
		public System.Int32 CategoryID {
			get { return _CategoryID; }
			set {
				_CategoryID = value;
				_CategoryIDChanged = true;
			}
		}

		private bool _CategoryNameChanged = false;
		private System.String _CategoryName;
		public System.String CategoryName {
			get { return _CategoryName; }
			set {
				_CategoryName = value;
				_CategoryNameChanged = true;
			}
		}

		private bool _DescriptionChanged = false;
		private System.String _Description;
		public System.String Description {
			get { return _Description; }
			set {
				_Description = value;
				_DescriptionChanged = true;
			}
		}

		private bool _PictureChanged = false;
		private System.Byte[] _Picture;
		public System.Byte[] Picture {
			get { return _Picture; }
			set {
				_Picture = value;
				_PictureChanged = true;
			}
		}

		public Categories() : this(null, null) { }

		public Categories(DbDataReader reader, DbConnection connection) {
			read(reader, connection);
		}

		public override void read(DbDataReader reader, DbConnection connection) {
			this.connection = connection;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (System.Int64)reader.GetValue(i); break;
					case "CategoryID": _CategoryID = (System.Int32)reader.GetValue(i); break;
					case "CategoryName": _CategoryName = reader.GetValue(i) as System.String; break;
					case "Description": _Description = reader.GetValue(i) as System.String; break;
					case "Picture": _Picture = reader.GetValue(i) as System.Byte[]; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_CategoryIDChanged)
				changed.Add("CategoryID", _CategoryID);
			if (_CategoryNameChanged)
				changed.Add("CategoryName", _CategoryName);
			if (_DescriptionChanged)
				changed.Add("Description", _Description);
			if (_PictureChanged)
				changed.Add("Picture", _Picture);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_CategoryID,
				_CategoryName,
				_Description,
				_Picture,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"CategoryID",
				"CategoryName",
				"Description",
				"Picture",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override System.Int64 getPKValue() {
			return _rowid;
		}

	}
}