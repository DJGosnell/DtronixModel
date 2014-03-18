using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DtxModel {
	public class Context : IDisposable {

		/// <summary>
		/// If true, the connection will be closed at this class destructor's call.
		/// </summary>
		protected bool owned_connection;

		/// <summary>
		/// The connection that this context tables will use.
		/// </summary>
		public DbConnection connection;

		/// <summary>
		/// Create a new context of this database's type.  Can only be used if a default connection is specified.
		/// </summary>
		public Context() { }

		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public Context(DbConnection connection) {
			this.connection = connection;
			owned_connection = false;
		}

		/// <summary>
		/// Releases any connection resources.
		/// </summary>
		public void Dispose() {
			if (owned_connection) {
				this.connection.Close();
				this.connection.Dispose();
			}
		}

	}
}
