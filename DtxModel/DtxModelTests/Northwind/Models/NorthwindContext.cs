using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using DtxModel;

namespace DtxModelTests.Northwind.Models {
	public class NorthwindContext : Context {
		private static Func<DbConnection> _default_connection = null;

		/// <summary>
		/// Set a default constructor to allow use of parameterless context calling.
		/// </summary>
		public static Func<DbConnection> DefaultConnection {
			get { return _default_connection; }
			set { _default_connection = value; }
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

			this.connection = _default_connection();

			if (this.connection == null) {
				throw new InvalidOperationException("Default connection lambda does not return a valid connection.");
			}

			owned_connection = true;
		}


		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public NorthwindContext(DbConnection connection) : base(connection) { }
	}
}
