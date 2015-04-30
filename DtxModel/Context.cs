using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DtxModel {
	public class Context : IDisposable {

		public enum TargetDb {
			MySql,
			Sqlite,
			Other
		}

		/// <summary>
		/// Enum to toggle each option to debug and output to the console.
		/// </summary>
		public enum DebugLevel {
			None,
			Queries,
			BoundParameters,
			All = Queries | BoundParameters
		}

		private DebugLevel _debug = DebugLevel.None;

		public DebugLevel Debug {
			get { return _debug; }
			set { _debug = value; }
		}


		/// <summary>
		/// If true, the connection will be closed at this class destructor's call.
		/// </summary>
		protected bool owned_connection;

		/// <summary>
		/// The connection that this context tables will use.
		/// </summary>
		public DbConnection Connection;

		/// <summary>
		/// Select string to be appended and read from insert queries to retrieve the newly inserted row's id.
		/// </summary>
		/// <remarks>
		/// This does not have to be set for databases used and set via the DatabaseType property.
		/// </remarks>
		public string LastInsertIdQuery;

		private bool _TransactionStarted;

		public bool TransactionStarted {
			get { return _TransactionStarted; }
		}


		/// <summary>
		/// Create a new context of this database's type.  Can only be used if a default connection is specified.
		/// </summary>
		public Context(Func<DbConnection> connection_callback, string last_insert_id_query) {
			if (connection_callback == null) {
				throw new Exception("No default connection has been specified for this type context.");
			}

			this.LastInsertIdQuery = last_insert_id_query;
			this.Connection = connection_callback();

			if (this.Connection.State == System.Data.ConnectionState.Closed) {
				this.Connection.Open();
			}

			if (this.Connection == null) {
				throw new InvalidOperationException("Default connection lambda does not return a valid connection.");
			}

			owned_connection = true;
		}

		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public Context(DbConnection connection, string last_insert_id_query) {
			this.LastInsertIdQuery = last_insert_id_query;
			this.Connection = connection;
			owned_connection = false;
		}


		/// <summary>
		/// Executes a string on the specified database.
		/// Will close the command after execution.
		/// </summary>
		/// <param name="sql">SQL to execute with parameters in string.format style.</param>
		/// <param name="binding">Parameters to replace the string.format placeholders with.</param>
		/// <returns>The number of rows affected.</returns>
		public int Query(string sql, params object[] binding) {
			return new SqlStatement<Model>(SqlStatement<Model>.Mode.Execute, this).Query(sql, binding);
		}

		/// <summary>
		/// Executes a string on the specified database and calls calls method with the reader.
		/// Will close the command after execution.
		/// </summary>
		/// <param name="sql">SQL to execute with parameters in string.format style.</param>
		/// <param name="binding">Parameters to replace the string.format placeholders with.</param>
		/// <param name="on_read">Called when the query has been executed and reader created.</param>
		/// <returns>The number of rows affected.</returns>
		public void QueryRead(string sql, object[] binding, Action<DbDataReader> on_read) {
			new SqlStatement<Model>(SqlStatement<Model>.Mode.Execute, this).QueryRead(sql, binding, on_read);
		}

		public DtxTransaction BeginTransaction() {
			if (_TransactionStarted) {
				throw new InvalidOperationException("Transaction has already been created.  Can not create nested transactions.");
			}
			_TransactionStarted = true;
			return new DtxTransaction(Connection.BeginTransaction(), () => { _TransactionStarted = false; });
		}

		/// <summary>
		/// Releases any connection resources.
		/// </summary>
		public void Dispose() {
			if (owned_connection) {
				this.Connection.Close();
				this.Connection.Dispose();
			}
		}

	}
}
