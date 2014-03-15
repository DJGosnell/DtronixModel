using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace DtxModelTests.Northwind.Models {
	class NorthwindContext : IDisposable {

		/// <summary>
		/// If true, the connection will be closed at this class destructor's call.
		/// </summary>
		private bool owned_connection;

		/// <summary>
		/// The connection that this context tables will use.
		/// </summary>
		public DbConnection connection;

		private static DbConnection _default_connection = null;

		/// <summary>
		/// Set a default constructor to allow use of parameterless context calling.
		/// </summary>
		public static DbConnection DefaultConnection {
			get { return _default_connection; }
			set {_default_connection = value; }
		}

		
		private Table<Customers> _customers;

		public Table<Customers> Customers {
			get {
				if (_customers == null) {
					_customers = new Table<Customers>(connection);
				}

				return _customers; 
			}
		}

		/// <summary>
		/// Create a new context of this database's type.  Can only be used if a default connection is specified.
		/// </summary>
		public NorthwindContext() {
			if (_default_connection == null) {
				throw new Exception("No default connection has been specified for this type context.");
			}

			// We have to determine what type of connection this context is connecting to.
			if (_default_connection is SQLiteConnection) {
				this.connection = new SQLiteConnection((SQLiteConnection)_default_connection);
				if (this.connection.State == System.Data.ConnectionState.Closed) {
					this.connection.Open();
				}

				// Ensure that this new connection will be closed at the dispose call.
				owned_connection = true;
			}

		}

		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public NorthwindContext(DbConnection connection) {
			this.connection = connection;
			owned_connection = false;
		}

		/// <summary>
		/// Releases any connection resources.
		/// </summary>
		public void Dispose(){
			if (owned_connection) {
				((SQLiteConnection)this.connection).Close();
				((SQLiteConnection)this.connection).Dispose();
				//GC.Collect();
			}
		}

	}
}
