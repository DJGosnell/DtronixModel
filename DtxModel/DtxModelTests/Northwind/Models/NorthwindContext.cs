using System;
using System.Data.Common;
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
		public NorthwindContext() : base(_default_connection) { }


		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public NorthwindContext(DbConnection connection) : base(connection) { }
	}
}
