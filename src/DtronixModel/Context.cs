using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DtronixModel
{
    /// <summary>
    /// Context to be derived into a specific database context. To be used inside a "using" statement.
    /// </summary>
    /// <example>
    /// using(var context = new Context()) {
    /// var result = context.Table.Select().Where("id = {0}", 1).ExecuteFetch();
    /// }
    /// </example>
    public abstract class Context : IDisposable
    {
        /// <summary>
        /// Enumeration to toggle each option to debug and output to the console.
        /// </summary>
        public enum DebugLevel
        {
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
        /// The default list of database targets for this context.
        /// </summary>
        public enum TargetDb
        {
            /// <summary>
            /// MySql server.
            /// </summary>
            MySql,

            /// <summary>
            /// Sqlite database.
            /// </summary>
            Sqlite,

            /// <summary>
            /// Used when connecting to a other type of db.
            /// </summary>
            Other
        }

        /// <summary>
        /// The connection that this context tables will use.
        /// </summary>
        public DbConnection Connection { get; private set; }


        /// <summary>
        /// Retrieves the current transaction if one exists for this context.  Null otherwise.
        /// </summary>
        public SqlTransaction Transaction { get; private set; }

        /// <summary>
        /// Set to the level of debugging to output.
        /// </summary>
        public DebugLevel Debug { get; set; } = DebugLevel.None;

        /// <summary>
        /// Select string to be appended and read from insert queries to retrieve the newly inserted row's id.
        /// </summary>
        /// <remarks>
        /// This does not have to be set for databases used and set via the DatabaseType property.
        /// </remarks>
        public string LastInsertIdQuery;

        /// <summary>
        /// If true, the connection will be closed at this class destructor's call.
        /// </summary>
        protected bool OwnedConnection;

        /// <summary>
        /// Create a new context of this database's type.  Can only be used if a default connection is specified.
        /// </summary>
        protected Context(Func<DbConnection> connectionCallback, string lastInsertIdQuery)
        {
            if (connectionCallback == null)
                throw new Exception("No default connection has been specified for this type context.");
            LastInsertIdQuery = lastInsertIdQuery;

            Connection = connectionCallback();

            OwnedConnection = true;
        }

        /// <summary>
        /// Create a new context of this database's type with a specific connection.
        /// </summary>
        /// <param name="connection">Existing open database connection to use.</param>
        /// <param name="lastInsertIdQuery">Sql to execute at the end of a insert query to retrieve the last ID inserted.</param>
        protected Context(DbConnection connection, string lastInsertIdQuery)
        {
            LastInsertIdQuery = lastInsertIdQuery;
            Connection = connection;
            OwnedConnection = false;
        }

        /// <summary>
        /// Opens the context to the database for usage.
        /// </summary>
        public void Open()
        {
            var openTask = OpenAsync(CancellationToken.None);
            openTask.Wait();
        }

        /// <summary>
        /// Opens the context to the database for usage asynchronously.
        /// </summary>
        public async Task OpenAsync(CancellationToken cancellationToken = default)
        {
            if (Connection.State == ConnectionState.Open)
                return;

            await Connection.OpenAsync(cancellationToken);

            if (Connection == null)
                throw new InvalidOperationException("Default connection lambda does not return a valid connection.");
        }

        /// <summary>
        /// Releases any connection resources.
        /// </summary>
        public void Dispose()
        {
            if (OwnedConnection)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        /// <summary>
        /// Sets the default connection creation method.
        /// </summary>
        /// <param name="defaultConnection">Method to be called on each context creation and return the new connection.</param>
        /// <param name="targetDb">Type of DB this is connecting to.</param>
        public abstract void SetDefaultConnection(Func<DbConnection> defaultConnection, TargetDb targetDb);

        /// <summary>
        /// Executes a string on the specified database.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <returns>The number of rows affected.</returns>
        public int Query(string sql, params object[] binding)
        {
            return new SqlStatement<GenericTableRow>(SqlStatement<GenericTableRow>.Mode.Execute, this).Query(sql, binding);
        }

        /// <summary>
        /// Executes a string on the specified database and calls calls method with the reader.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <param name="onRead">Called when the query has been executed and reader created.</param>
        /// <returns>The number of rows affected.</returns>
        public void QueryRead(string sql, object[] binding, Action<DbDataReader> onRead)
        {
            new SqlStatement<GenericTableRow>(SqlStatement<GenericTableRow>.Mode.Execute, this).QueryRead(sql, binding, onRead);
        }

        /// <summary>
        /// Executes a string on the specified database.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> QueryAsync(string sql, object[] binding, CancellationToken cancellationToken = default)
        {
            return await new SqlStatement<GenericTableRow>(SqlStatement<GenericTableRow>.Mode.Execute, this)
                .QueryAsync(sql, binding, cancellationToken);
        }

        /// <summary>
        /// Executes a string on the specified database and calls calls method with the reader.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <param name="onRead">Called when the query has been executed and reader created.</param>
        /// <param name="cancellationToken">>Cancellation token.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task QueryReadAsync(
            string sql, 
            object[] binding,
            Func<DbDataReader, CancellationToken, Task> onRead,
            CancellationToken cancellationToken = default)
        {
            await new SqlStatement<GenericTableRow>(SqlStatement<GenericTableRow>.Mode.Execute, this)
                .QueryReadAsync(sql, binding, onRead, cancellationToken);
        }

        /// <summary>
        /// Begins a transaction on this context. Use within a "using" statement
        /// </summary>
        /// <example>
        /// using (var transaction = context.BeginTransaction()) {
        /// Normal query/updates/inserts.
        /// }
        /// </example>
        /// <returns>Wrapped transaction.</returns>
        public SqlTransaction BeginTransaction()
        {
            if (Transaction != null)
                throw new InvalidOperationException(
                    "Transaction has already been created.  Can not create nested transactions.");

            return Transaction = new SqlTransaction(Connection.BeginTransaction(), () => { Transaction = null; });
        }
    }
}