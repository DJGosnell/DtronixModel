using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModelTests.Northwind.Models {
	class NorthwindContext {
		private CustomersTable _customers;

		public CustomersTable Customers {
			get { return _customers; }
			set { _customers = value; }
		}

	}
}
