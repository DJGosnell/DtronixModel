using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using DtxModel;

namespace DtxModelTests.Northwind.Models {
    class Customers : IModel {

        private SQLiteConnection connection;

        private long _rowid;

        public long rowid {
            get { return _rowid; }
            set { _rowid = value; }
        }

        private string _CustomerID;

        public string CustomerID {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }

        private string _CompanyName;

        public string CompanyName {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

        private string _ContactName;

        public string ContactName {
            get { return _ContactName; }
            set { _ContactName = value; }
        }

        private string _Address;

        public string Address {
            get { return _Address; }
            set { _Address = value; }
        }

        private string _City;

        public string City {
            get { return _City; }
            set { _City = value; }
        }

        private string _Region;

        public string Region {
            get { return _Region; }
            set { _Region = value; }
        }

        private string _PostalCode;

        public string PostalCode {
            get { return _PostalCode; }
            set { _PostalCode = value; }
        }

        private string _Country;

        public string Country {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _Phone;

        public string Phone {
            get { return _Phone; }
            set { _Phone = value; }
        }

        private string _Fax;

        public string Fax {
            get { return _Fax; }
            set { _Fax = value; }
        }

        public Customers(SQLiteConnection connection) {
            this.connection = connection;
        }

        public SqlStatement select() {
            return new SqlStatement(SqlStatement.Mode.Select, connection);
        }

		protected Customers readOne(System.Data.SqlClient.SqlDataReader reader) {
			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = reader.GetInt64(i); break;
					case "CustomerID": _CustomerID = reader.GetString(i); break;
					case "CompanyName": _CompanyName = reader.GetString(i); break;
					case "ContactName": _ContactName = reader.GetString(i); break;
					case "Address": _Address = reader.GetString(i); break;
					case "City": _City = reader.GetString(i); break;
					case "Region": _Region = reader.GetString(i); break;
					case "PostalCode": _PostalCode = reader.GetString(i); break;
					case "Country": _Country = reader.GetString(i); break;
					case "Phone": _Phone = reader.GetString(i); break;
					case "Fax": _Fax = reader.GetString(i); break;
					default:
						break;
				}
			}
			return this;

		}

		protected Customers[] readAll(System.Data.SqlClient.SqlDataReader reader) {

			var rows = new List<Customers>();

			while (reader.Read()) {
				var row = new Customers(connection);
				row.readOne(reader);
				rows.Add(row);
			}

			return rows.ToArray();

		}
    }
}
