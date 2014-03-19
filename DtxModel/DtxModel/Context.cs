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
		/// Executes a string on the specified database.
		/// Will close the command after execution.
		/// </summary>
		/// <param name="sql">SQL to execute with parameters in string.format style.</param>
		/// <param name="binding">Parameters to replace the string.format placeholders with.</param>
		/// <returns>The number of rows affected.</returns>
		public int query(string sql, params object[] binding) {
			return new SqlStatement<Model>(SqlStatement<Model>.Mode.Execute, connection).query(sql, binding);
		}

		/// <summary>
		/// Executes a string on the specified database and calls calls method with the reader.
		/// Will close the command after execution.
		/// </summary>
		/// <param name="sql">SQL to execute with parameters in string.format style.</param>
		/// <param name="binding">Parameters to replace the string.format placeholders with.</param>
		/// <param name="on_read">Called when the query has been executed and reader created.</param>
		/// <returns>The number of rows affected.</returns>
		public void queryRead(string sql, object[] binding, Action<DbDataReader> on_read) {
			new SqlStatement<Model>(SqlStatement<Model>.Mode.Execute, connection).queryRead(sql, binding, on_read);
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
