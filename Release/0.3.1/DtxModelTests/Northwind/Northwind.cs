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
					_Customers = new Table<Customers>(this);
				}

				return _Customers;}
		}

		private Table<Categories> _Categories;

		public Table<Categories> Categories {
			get {
				if(_Categories == null) {
					_Categories = new Table<Categories>(this);
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

		private bool _PhoneChanged = false;
		private System.String _Phone;
		public System.String Phone {
			get { return _Phone; }
			set {
				_Phone = value;
				_PhoneChanged = true;
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

		private Categories _Category;
		public Categories Category {
			get {
				if(_Category == null){ 
					try {
						_Category = ((NorthwindContext)context).Categories.Select().WhereIn("CategoryID", Categories_rowid).ExecuteFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Category = null;
					}
				}
				return _Category;
			}
		}

		public Customers() : this(null, null) { }

		public Customers(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				object value = reader.GetValue(i);
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int64) : reader.GetInt64(i); break;
					case "CustomerID": _CustomerID = reader.GetValue(i) as System.String; break;
					case "CompanyName": _CompanyName = reader.GetValue(i) as System.String; break;
					case "ContactName": _ContactName = reader.GetValue(i) as System.String; break;
					case "ContactTitle": _ContactTitle = reader.GetValue(i) as System.String; break;
					case "Address": _Address = reader.GetValue(i) as System.String; break;
					case "City": _City = reader.GetValue(i) as System.String; break;
					case "Region": _Region = reader.GetValue(i) as System.String; break;
					case "PostalCode": _PostalCode = reader.GetValue(i) as System.String; break;
					case "Country": _Country = reader.GetValue(i) as System.String; break;
					case "Phone": _Phone = reader.GetValue(i) as System.String; break;
					case "Fax": _Fax = reader.GetValue(i) as System.String; break;
					case "Categories_rowid": _Categories_rowid = (reader.IsDBNull(i)) ? default(System.Int64) : reader.GetInt64(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
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
			if (_PhoneChanged)
				changed.Add("Phone", _Phone);
			if (_FaxChanged)
				changed.Add("Fax", _Fax);
			if (_Categories_rowidChanged)
				changed.Add("Categories_rowid", _Categories_rowid);

			return changed;
		}

		public override object[] GetAllValues() {
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
				_Phone,
				_Fax,
				_Categories_rowid,
			};
		}

		public override string[] GetColumns() {
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
				"Phone",
				"Fax",
				"Categories_rowid",
			};
		}

		public override string GetPKName() {
			return "rowid";
		}

		public override object GetPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "Categories")]
	public class Categories : Model {
		private System.Int64 _CategoryID;
		public System.Int64 CategoryID {
			get { return _CategoryID; }
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

		private Customers[] _Customers;
		public Customers[] Customers {
			get {
				if(_Customers == null){ 
					try {
						_Customers = ((NorthwindContext)context).Customers.Select().WhereIn("Categories_rowid", CategoryID).ExecuteFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Customers = null;
					}
				}
				return _Customers;
			}
		}

		public Categories() : this(null, null) { }

		public Categories(DbDataReader reader, Context context) {
			Read(reader, context);
		}

		public override void Read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				object value = reader.GetValue(i);
				switch (reader.GetName(i)) {
					case "CategoryID": _CategoryID = (reader.IsDBNull(i)) ? default(System.Int64) : reader.GetInt64(i); break;
					case "CategoryName": _CategoryName = reader.GetValue(i) as System.String; break;
					case "Description": _Description = reader.GetValue(i) as System.String; break;
					case "Picture": _Picture = reader.GetValue(i) as System.Byte[]; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> GetChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_CategoryNameChanged)
				changed.Add("CategoryName", _CategoryName);
			if (_DescriptionChanged)
				changed.Add("Description", _Description);
			if (_PictureChanged)
				changed.Add("Picture", _Picture);

			return changed;
		}

		public override object[] GetAllValues() {
			return new object[] {
				_CategoryName,
				_Description,
				_Picture,
			};
		}

		public override string[] GetColumns() {
			return new string[] {
				"CategoryName",
				"Description",
				"Picture",
			};
		}

		public override string GetPKName() {
			return "CategoryID";
		}

		public override object GetPKValue() {
			return _CategoryID;
		}

	}
}