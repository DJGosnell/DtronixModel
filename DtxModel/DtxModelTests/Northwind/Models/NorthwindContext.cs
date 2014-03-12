using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DtxModelTests.Northwind.Models {
	class NorthwindContext : IDisposable {
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

		public NorthwindContext(DbConnection connection) {
			this.connection = connection;

		}


		public void Dispose(){

		}

	}
}
