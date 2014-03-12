using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace DtxModelTests.Northwind.Models {
	class NorthwindContext : IDisposable {

		private bool owned_connection;

		private static DbConnection _default_connection = null;

		public static DbConnection DefaultConnection {
			get { return _default_connection; }
			set {
				_default_connection = value; 
			}
		}

		public DbConnection connection;
		private Table<Customers> _customers;

		public Table<Customers> Customers {
			get {
				if (_customers == null) {
					_customers = new Table<Customers>(connection, "Customers");
				}

				return _customers; 
			}
		}

		public NorthwindContext() {
			if (_default_connection == null) {
				throw new Exception("No default connection has been specified for this type context.");
			}

			if (_default_connection is SQLiteConnection) {
				this.connection = new SQLiteConnection((SQLiteConnection)_default_connection);
				if (this.connection.State == System.Data.ConnectionState.Closed) {
					this.connection.Open();
				}
				owned_connection = true;
			}

		}

		public NorthwindContext(DbConnection connection) {
			this.connection = connection;
			owned_connection = false;
		}


		public void Dispose(){
			if (owned_connection) {
				this.connection.Close();
			}
		}

	}
}
