using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DtronixModel {

	/// <summary>
	/// Context to be derived into a specific database context. To be used inside a "using" statement.
	/// </summary>
	/// <example>
	/// using(var context = new Context()) {
	///		var result = context.Table.Select().Where("id = {0}", 1).ExecuteFetch();
	/// }
	/// </example>
	public class Context : IDisposable {

		/// <summary>
		/// The default list of database targets for this context.
		/// </summary>
		public enum TargetDb {
			MySql,
			Sqlite,
			Other
		}

		/// <summary>
		/// Enumeration to toggle each option to debug and output to the console.
		/// </summary>
		public enum DebugLevel {
			/// <summary>
			/// (Default) No output is generated.
			/// </summary>
			None = 0,

			/// <summary>
			/// Logs the executed queries to the stdout
			/// </summary>
			Queries = 1 << 1,

			/// <summary>
			/// Logs the parameters as they are being bound to stdout.
			/// </summary>
			BoundParameters = 1 << 2,

			/// <summary>
			/// Logs the rows as they are inserted to stdout.
			/// </summary>
			Inserts = 1 << 3,

			/// <summary>
			/// Logs the rows as they are updated to stdout.
			/// </summary>
			Updates = 1 << 4,

			/// <summary>
			/// Logs every option above to stdout.
			/// </summary>
			All = Queries | BoundParameters | Inserts | Updates
		}

		/// <summary>
		/// Set to the level of debugging to output.  
		/// </summary>
		public DebugLevel Debug { get; set; } = DebugLevel.None;


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

		/// <summary>
		/// True if a transaction has been started.
		/// </summary>
		private bool _TransactionStarted;

		/// <summary>
		/// Retrieves if this context is in a transaction or not.
		/// </summary>
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

		/// <summary>
		/// Begins a transaction on this context. Use within a "using" statement
		/// </summary>
		/// <example>
		/// using (var transaction = context.BeginTransaction()) {
		///		Normal query/updates/inserts.
		/// }
		/// </example>
		/// <returns>Wrapped transaction.</returns>
		public DtronixTransaction BeginTransaction() {
			if (_TransactionStarted) {
				throw new InvalidOperationException("Transaction has already been created.  Can not create nested transactions.");
			}
			_TransactionStarted = true;
			return new DtronixTransaction(Connection.BeginTransaction(), () => { _TransactionStarted = false; });
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
