using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DtronixModel
{
    /// <summary>
    /// Class to help in quick and simple CRUD operations on a database table.
    /// </summary>
    /// <typeparam name="T">Model this class will be working with.</typeparam>
    public class SqlStatement<T> : IDisposable
        where T : TableRow, new()
    {
        /// <summary>
        /// Mode this statement will be bound to.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Setup in execution mode.
            /// </summary>
            Execute,

            /// <summary>
            /// Setup in selection mode.
            /// </summary>
            Select,

            /// <summary>
            /// Setup in insertion mode.
            /// </summary>
            Insert,

            /// <summary>
            /// Setup in Update mode.
            /// </summary>
            Update,

            /// <summary>
            /// Setup in deletion mode.
            /// </summary>
            Delete
        }

        private readonly DbCommand _command;


        private readonly Context _context;

        /// <summary>
        /// Internal mode that the statement was setup with.
        /// </summary>
        private readonly Mode _mode;

        /// <summary>
        /// Name of the table this statement is querying.
        /// </summary>
        private readonly string _tableName;

        /// <summary>
        /// List containing groupings for the query.
        /// </summary>
        private List<string> _sqlGroups;

        /// <summary>
        /// Limits the number of returned rows in the query. -1 for no limits.
        /// </summary>
        private int _sqlLimitCount = -1;

        /// <summary>
        /// Offset for the query limits. -1 for no offset.
        /// </summary>
        private int _sqlLimitOffset = -1;

        /// <summary>
        /// Model array of the current rows to be inserted, deleted or updated.
        /// </summary>
        private T[] _sqlRows;

        /// <summary>
        /// Contains a dictionary list of the sort orders for this query.
        /// </summary>
        private Dictionary<string, SortDirection> _sqlOrders;

        /// <summary>
        /// Holds the columns to select.
        /// </summary>
        private string _sqlSelect = "*";

        /// <summary>
        /// Contains the bound where portion of the query.
        /// </summary>
        private string _sqlWhere;


        /// <summary>
        /// Starts a Statement in the specified mode of operation. Always dispose of the statement to ensure underlying
        /// DbCommand is disposed and prevent memory leaks.
        /// </summary>
        /// <param name="mode">Mode that this query will operate in. Prevents invalid operations.</param>
        /// <param name="context">Context that this query will operate inside of.</param>
        public SqlStatement(Mode mode, Context context)
        {
            _context = context;
            _mode = mode;
            _command = context.Connection.CreateCommand();

            try
            {
                if (mode != Mode.Execute)
                    _tableName = AttributeCache<T, TableAttribute>.GetAttribute().Name;
            }
            catch (Exception)
            {
                throw new Exception("Class passed does not have a TableAttribute");
            }
        }

        /// <summary>
        /// True to close the command at the end of the query.
        /// </summary>
        public bool AutoCloseCommand { get; set; } = true;

        void IDisposable.Dispose()
        {
            _command.Dispose();
        }

        /// <summary>
        /// Executes a string on the specified database.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <returns>The number of rows affected.</returns>
        public int Query(string sql, params object[] binding)
        {
            var queryTask = QueryAsync(sql, binding, CancellationToken.None);

            return queryTask.Result;
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
            if (_mode != Mode.Execute)
                throw new InvalidOperationException("Need to be in Execute mode to use this method.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            _command.Parameters.Clear();
            _command.CommandText = SqlBindParameters(sql, binding);

            // Logging to output queries to stdout.
            if (_context.Debug.HasFlag(Context.DebugLevel.Queries))
                Console.Out.WriteLine("Query: \r\n" + _command.CommandText);

            var result = await _command.ExecuteNonQueryAsync(cancellationToken);

            _command.Dispose();

            return result;
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
            var queryReadTask = QueryReadAsync(sql, binding, (reader, ct) =>
            {
                onRead(reader);
                return Task.CompletedTask;
            }, CancellationToken.None);

            queryReadTask.Wait();
        }

        /// <summary>
        /// Executes a string on the specified database and calls calls method with the reader.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <param name="onRead">Called when the query has been executed and reader created.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task QueryReadAsync(
            string sql, 
            object[] binding, 
            Func<DbDataReader, CancellationToken, Task> onRead, 
            CancellationToken cancellationToken = default)
        {
            if (_mode != Mode.Execute)
                throw new InvalidOperationException("Need to be in Execute mode to use this method.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            _command.Parameters.Clear();
            _command.CommandText = SqlBindParameters(sql, binding);

            // Logging to output queries to stdout.
            if (_context.Debug.HasFlag(Context.DebugLevel.Queries))
                Console.Out.WriteLine("Query: \r\n" + _command.CommandText);

            using (var reader = await _command.ExecuteReaderAsync(cancellationToken))
                await onRead(reader, cancellationToken);

            _command.Dispose();
        }


        /// <summary>
        /// Begins selection process and the specifies columns to return from the database.
        /// </summary>
        /// <param name="select">Columns to select.  Selecting "*" will select all the columns in the table</param>
        /// <returns>Current statement for chaining.</returns>
        public SqlStatement<T> Select(string select)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            _sqlSelect = select;
            return this;
        }

        /// <summary>
        /// Updates the specified rows in the database. The rows must have their primary keys set.
        /// </summary>
        /// <param name="models">Rows to update with their new values.</param>
        public void Update(T[] models)
        {
            var updateTask = UpdateAsync(models, CancellationToken.None);
            updateTask.Wait();
        }

        /// <summary>
        /// Updates the specified rows in the database. The rows must have their primary keys set.
        /// </summary>
        /// <param name="models">Rows to update with their new values.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task UpdateAsync(T[] models, CancellationToken cancellationToken = default)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            _sqlRows = models;
            await ExecuteAsync(cancellationToken);
            _command.Dispose();
        }


        /// <summary>
        /// Deletes the specified rows from the database. The rows must have their primary keys set.
        /// </summary>
        /// <param name="models">Rows to delete.</param>
        public void Delete(T[] models)
        {
            var deleteTask = DeleteAsync(models, CancellationToken.None);
            deleteTask.Wait();
        }

        /// <summary>
        /// Deletes the specified primary keys from the table.
        /// </summary>
        /// <param name="primaryIds">Ids to delete.</param>
        public void Delete(long[] primaryIds)
        {
            var deleteTask = DeleteAsync(primaryIds, CancellationToken.None);

            deleteTask.Wait();
        }

        /// <summary>
        /// Deletes the specified primary keys from the table.
        /// </summary>
        /// <param name="primaryIds">Ids to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task DeleteAsync(long[] primaryIds, CancellationToken cancellationToken = default)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            WhereIn(new T().GetPKName(), primaryIds.Cast<object>().ToArray());
            await ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes the specified rows from the database. The rows must have their primary keys set.
        /// </summary>
        /// <param name="models">Rows to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task DeleteAsync(T[] models, CancellationToken cancellationToken = default)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            Where(models);

            _sqlRows = models;
            await ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Specifies a column to match the specified values.
        /// </summary>
        /// <param name="column">Column to match against.</param>
        /// <param name="values">Values to check against the specified column.</param>
        /// <returns>Current statement for chaining.</returns>
        public SqlStatement<T> WhereIn(string column, object[] values)
        {
            ValidateWhere();

            if (string.IsNullOrWhiteSpace(column))
                throw new ArgumentException("Column parameter can not be empty.");

            var sql = new StringBuilder();
            sql.Append(column).Append(" IN(");

            foreach (var value in values)
                sql.Append(BindParameter(value)).Append(",");
            sql.Remove(sql.Length - 1, 1).Append(")");

            _sqlWhere = sql.ToString();

            return this;
        }

        /// <summary>
        /// Sets where to the provided rows's primary key.
        /// </summary>
        /// <param name="model">Row to provide the primary key for.</param>
        /// <returns>Current statement for chaining.</returns>
        public SqlStatement<T> Where(T model)
        {
            return Where(new[] { model });
        }

        /// <summary>
        /// Sets where to the provided row's primary keys.
        /// </summary>
        /// <param name="models">Row to provide the primary key for.</param>
        /// <returns>Current statement for chaining.</returns>
        public SqlStatement<T> Where(T[] models)
        {
            ValidateWhere();

            // Set the update by the primary key.
            if (models == null || models.Length == 0)
                throw new ArgumentException("Models parameter can not be null or empty.");

            // Get the primary key for the first parameter 
            var pkName = models[0].GetPKName();

            var sql = new StringBuilder();
            sql.Append(pkName).Append(" IN(");

            foreach (var model in models)
                sql.Append(BindParameter(model.GetPKValue())).Append(",");
            sql.Remove(sql.Length - 1, 1).Append(")");

            _sqlWhere = sql.ToString();

            return this;
        }


        /// <summary>
        /// Specifies a custom where string to be applied to the query. Use the String.Format type arguments for this method.
        /// </summary>
        /// <param name="where">
        /// Where string to apply to the query. Use String.Format holders ({0}, {1}, etc...) for the bound
        /// parameters.
        /// </param>
        /// <param name="parameters">Parameters to bind to the query.</param>
        /// <returns>Current statement for chaining.</returns>
        public SqlStatement<T> Where(string where, params object[] parameters)
        {
            ValidateWhere();

            _sqlWhere = SqlBindParameters(where, parameters);

            return this;
        }

        /// <summary>
        /// Limits the rows returned by the server.
        /// </summary>
        /// <param name="count">Number of rows to return.</param>
        /// <returns>Current statement for chaining.</returns>
        public SqlStatement<T> Limit(int count)
        {
            return Limit(count, -1);
        }

        /// <summary>
        /// Limits the rows returned by the server.
        /// </summary>
        /// <param name="count">Number of rows to return.</param>
        /// <param name="offset">Number of rows offset the counter into the return set.</param>
        /// <returns>Current statement for chaining.</returns>
        public SqlStatement<T> Limit(int count, int offset)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            if (_mode != Mode.Select)
                throw new InvalidOperationException("Can not use the LIMIT method except in SELECT mode.");

            _sqlLimitOffset = offset;
            _sqlLimitCount = count;

            return this;
        }

        /// <summary>
        /// Orders the found results by the specified column.  Call multiple times to specify multiple orders.
        /// </summary>
        /// <param name="column">Column to order.</param>
        /// <param name="direction">Direction to order the specified column.</param>
        /// <returns>Current statement for chaining.</returns>
        public SqlStatement<T> OrderBy(string column, SortDirection direction)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            if (_mode != Mode.Select)
                throw new InvalidOperationException("Can not use the ORDER BY method except in SELECT mode.");

            if (_sqlOrders == null)
                _sqlOrders = new Dictionary<string, SortDirection>();

            _sqlOrders.Add(column, direction);

            return this;
        }

        /// <summary>
        /// Groups the statement by the specified column.  Call multiple times to specify multiple groups.
        /// </summary>
        /// <param name="column">Column to add to the group statement.</param>
        /// <returns>Current statement for chaining.</returns>
        public SqlStatement<T> GroupBy(string column)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            if (_mode != Mode.Select)
                throw new InvalidOperationException("Can not use the GROUP BY method except in SELECT mode.");

            if (_sqlGroups == null)
                _sqlGroups = new List<string>();

            _sqlGroups.Add(column);

            return this;
        }

        /// <summary>
        /// Executes the query built.
        /// </summary>
        public void Execute()
        {
            ExecuteAsync(CancellationToken.None).Wait();
        }

        /// <summary>
        /// Executes the query built.
        /// </summary>
        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            if (_mode == Mode.Update)
            {

                SqlTransaction transaction = null;
                try
                {
                    // Start a transaction if one does not already exist.
                    if (_context.Transaction == null)
                        transaction = _context.BeginTransaction();

                    foreach (var model in _sqlRows)
                    {
                        _sqlWhere = null;
                        Where(model);
                        BuildSql(model);

                        // Execute the update command.
                        await _command.ExecuteNonQueryAsync(cancellationToken);

                        if (_context.Debug.HasFlag(Context.DebugLevel.Updates))
                            Console.Out.WriteLine("Update: \r\n" + _command.CommandText);
                    }

                    transaction?.Commit();
                }
                finally
                {
                    transaction?.Dispose();
                }
            }
            else
            {
                BuildSql(null);
                await _command.ExecuteNonQueryAsync(cancellationToken);
            }

            if (AutoCloseCommand)
                _command.Dispose();
        }

        /// <summary>
        /// Executes the query built and returns the associated rows with the query. Synchronous.
        /// </summary>
        /// <returns>On success, returns rows with the result of the query; Otherwise returns null.</returns>
        public T ExecuteFetch()
        {
            var fetchTask = ExecuteFetchAsync(CancellationToken.None);

            return fetchTask.Result;
        }

        /// <summary>
        /// Executes the query built and returns the associated rows with the query. Synchronous.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>On success, returns rows with the result of the query; Otherwise returns null.</returns>
        public async Task<T> ExecuteFetchAsync(CancellationToken cancellationToken = default)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            if (_mode != Mode.Select)
                throw new InvalidOperationException("Can not fetch from the server when not in SELECT mode.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            BuildSql(null);
            T model;

            using (var reader = await _command.ExecuteReaderAsync(cancellationToken))
            {
                if (await reader.ReadAsync(cancellationToken) == false)
                    return default;

                model = new T();
                model.Read(reader, _context);
            }

            if (AutoCloseCommand)
                _command.Dispose();

            return model;
        }

        /// <summary>
        ///  Executes the query built and returns the associated rows with the query. Synchronous.
        /// </summary>
        /// <returns>On success, returns rows with the result of the query; Otherwise returns an empty array.</returns>
        public T[] ExecuteFetchAll()
        {
            return ExecuteFetchAll(null, null);
        }


        /// <summary>
        ///  Executes the specified query and returns the associated rows with the query. Synchronous.
        /// </summary>
        /// <param name="query">Query override which will ignore all previous query builder commands.</param>
        /// <returns>On success, returns rows with the result of the query; Otherwise returns an empty array.</returns>
        public T[] ExecuteFetchAll(string query)
        {
            return ExecuteFetchAll(query, null);
        }


        /// <summary>
        ///  Executes the specified query, binds the passed parameters and returns the associated rows with the query. Synchronous.
        /// </summary>
        /// <param name="query">Query override which will ignore all previous query builder commands.</param>
        /// <param name="parameters">Parameters to bind to the query.</param>
        /// <returns>On success, returns rows with the result of the query; Otherwise returns an empty array.</returns>
        public T[] ExecuteFetchAll(string query, object[] parameters)
        {
            var fetchTask = ExecuteFetchAllAsync(query, parameters, CancellationToken.None);

            fetchTask.Wait();

            return fetchTask.Result;
        }

        /// <summary>
        ///  Executes the query built and returns the associated rows with the query. Synchronous.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>On success, returns rows with the result of the query; Otherwise returns an empty array.</returns>
        public async Task<T[]> ExecuteFetchAllAsync(CancellationToken cancellationToken = default)
        {
            return await ExecuteFetchAllAsync(null, null, cancellationToken);
        }

        /// <summary>
        ///  Executes the specified query and returns the associated rows with the query. Synchronous.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="query">Query override which will ignore all previous query builder commands.</param>
        /// <returns>On success, returns rows with the result of the query; Otherwise returns an empty array.</returns>
        public async Task<T[]> ExecuteFetchAllAsync(string query, CancellationToken cancellationToken = default)
        {
            return await ExecuteFetchAllAsync(query, null, cancellationToken);
        }

        /// <summary>
        ///  Executes the specified query, binds the passed parameters and returns the associated rows with the query. Synchronous.
        /// </summary>
        /// <param name="query">Query override which will ignore all previous query builder commands.</param>
        /// <param name="parameters">Parameters to bind to the query.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>On success, returns rows with the result of the query; Otherwise returns an empty array.</returns>
        public async Task<T[]> ExecuteFetchAllAsync(string query, object[] parameters, CancellationToken cancellationToken = default)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            if (_mode != Mode.Select)
                throw new InvalidOperationException("Can not fetch from the server when not in SELECT mode.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            if (query == null)
                BuildSql(null);
            else
            {
                // Clear all previous bound parameters.
                _command.Parameters.Clear();
                _command.CommandText = SqlBindParameters(query, parameters);
            }

            var results = new List<T>();
            using (var reader = await _command.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    var model = new T();
                    model.Read(reader, _context);
                    results.Add(model);
                }
            }

            if (AutoCloseCommand)
                _command.Dispose();

            return results.ToArray();
        }

        /// <summary>
        /// Builds the SQL statement that this class currently represents.
        /// </summary>
        /// <param name="model">Model to base this query on.</param>
        private void BuildSql(T model)
        {
            var sql = new StringBuilder();

            switch (_mode)
            {
                case Mode.Select:
                    sql.Append("SELECT ").AppendLine(_sqlSelect);
                    sql.Append("FROM ").AppendLine(_tableName);
                    break;
                case Mode.Insert:
                    throw new InvalidOperationException("Can not build an SQL query in the INSERT mode.");
                case Mode.Update:
                    sql.Append("UPDATE ").AppendLine(_tableName);
                    sql.Append("SET ");

                    var changedFields = model.GetChangedValues();

                    // If there are no fields to update, then do nothing.
                    if (changedFields.Count == 0)
                        sql.Clear();

                    foreach (var field in changedFields)
                        sql.Append(field.Key).Append(" = ").Append(BindParameter(field.Value)).Append(", ");

                    sql.Remove(sql.Length - 2, 2).AppendLine();
                    break;
                case Mode.Delete:
                    sql.Append("DELETE FROM ").AppendLine(_tableName);
                    break;

                case Mode.Execute:
                    throw new InvalidOperationException("Can not use all functions in Execute mode.");
            }


            // WHERE
            if (_mode != Mode.Insert && _sqlWhere != null)
                sql.Append("WHERE ").AppendLine(_sqlWhere);

            // GROUP BY
            if (_mode == Mode.Select && _sqlGroups != null)
            {
                sql.Append("GROUP BY ");
                foreach (var groupColumn in _sqlGroups)
                    sql.Append(groupColumn).Append(", ");
                sql.Remove(sql.Length - 2, 2).AppendLine();
            }

            // ORDER BY
            if (_mode == Mode.Select && _sqlOrders != null)
            {
                sql.Append("ORDER BY ");
                foreach (var orderColumn in _sqlOrders.Keys)
                {
                    sql.Append(orderColumn);

                    switch (_sqlOrders[orderColumn])
                    {
                        case SortDirection.Ascending:
                            sql.Append(" ASC, ");
                            break;
                        case SortDirection.Descending:
                            sql.Append(" DESC, ");
                            break;
                    }
                }

                // Remove the trailing ", "
                sql.Remove(sql.Length - 2, 2).AppendLine();
            }

            if (_mode == Mode.Select && _sqlLimitCount != -1)
            {
                sql.Append("LIMIT ");

                if (_sqlLimitOffset != -1)
                    sql.Append(_sqlLimitOffset).Append(", ");

                sql.Append(_sqlLimitCount);
            }

            _command.CommandText = sql.ToString();
            if (_context.Debug.HasFlag(Context.DebugLevel.Queries))
                Console.Out.WriteLine("Query: \r\n" + _command.CommandText);
        }


        /// <summary>
        /// Binds a parameter in the current command.
        /// </summary>
        /// <param name="value">Value to bind.</param>
        /// <param name="parameterList">List of paramaters to bind.</param>
        /// <returns>Parameter name for the binding reference.</returns>
        private string BindParameter(object value, List<DbParameter> parameterList = null)
        {
            var key = "@p" + _command.Parameters.Count;
            var param = _command.CreateParameter();
            param.ParameterName = key;
            param.Value = PrepareParameterValue(value);

            // Logging to output bound parameters to stdout.
            if (_context.Debug.HasFlag(Context.DebugLevel.BoundParameters))
                Console.Out.WriteLine("Parameter: " + key + " = " + value);

            parameterList?.Add(param);

            _command.Parameters.Add(param);
            return key;
        }

        /// <summary>
        /// Prepares parameter values by translating them into their proper value types.
        /// </summary>
        /// <param name="value">Parameter to prepare.</param>
        /// <returns>Prepared parameter.</returns>
        private object PrepareParameterValue(object value)
        {
            if (value is DateTimeOffset)
                value = ((DateTimeOffset)value).ToString("o");

            return value;
        }

        /// <summary>
        /// Inserts multiple rows into the database.
        /// </summary>
        /// <remarks>
        /// This method by default wraps all inserts into a transaction.
        /// If one of the inserts fails, then all of the inserts are rolled back.
        /// </remarks>
        /// <param name="models">Rows to insert.</param>
        /// <returns>Inserted IDs of the rows.</returns>
        public long[] Insert(T[] models)
        {
            var inserTask = InsertAsync(models, CancellationToken.None);

            return inserTask.Result;
        }

        /// <summary>
        /// Inserts multiple rows into the database.
        /// </summary>
        /// <remarks>
        /// This method by default wraps all inserts into a transaction.
        /// If one of the inserts fails, then all of the inserts are rolled back.
        /// </remarks>
        /// <param name="models">Rows to insert.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Inserted IDs of the rows.</returns>
        public async Task<long[]> InsertAsync(T[] models, CancellationToken cancellationToken = default)
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            if (models == null || models.Length == 0)
                throw new ArgumentException("Model array is empty.");

            if (_mode != Mode.Insert)
                throw new InvalidOperationException("Can not insert when statement is not in INSERT mode.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            var columns = models[0].GetColumns();

            var sbSql = new StringBuilder();
            sbSql.Append("INSERT INTO ").Append(_tableName).Append(" (");

            // Add all the column names.
            foreach (var column in columns)
                sbSql.Append("").Append(column).Append(", ");

            // Remove the last ", " from the query.
            sbSql.Remove(sbSql.Length - 2, 2);

            // Add the values.
            sbSql.Append(") VALUES (");
            for (var i = 0; i < columns.Length; i++)
                sbSql.Append("@v").Append(i).Append(", ");

            // Remove the last ", " from the query.
            sbSql.Remove(sbSql.Length - 2, 2);
            sbSql.Append(");");

            long[] newRowIds = null;

            if (_context.LastInsertIdQuery != null)
            {
                sbSql.Append(_context.LastInsertIdQuery);
                newRowIds = new long[models.Length];
            }
            SqlTransaction transaction = null;

            try
            {
                // Start a transaction if one does not already exist for fast bulk inserts.
                if (_context.Transaction == null)
                    transaction = _context.BeginTransaction();

                _command.CommandText = sbSql.ToString();
                _command.Transaction = _context.Transaction.Transaction;

                // Create the parameters for bulk inserts.
                for (var i = 0; i < columns.Length; i++)
                {
                    var parameter = _command.CreateParameter();
                    parameter.ParameterName = "@v" + i;
                    _command.Parameters.Add(parameter);
                }

                // Loop through watch of the provided models.
                for (var i = 0; i < models.Length; i++)
                {
                    var values = models[i].GetAllValues();

                    for (var x = 0; x < values.Length; x++)
                        _command.Parameters[x].Value = PrepareParameterValue(values[x]);

                    if (_context.LastInsertIdQuery != null)
                    {
                        var newRow = await _command.ExecuteScalarAsync(cancellationToken);
                        if (newRow == null)
                            throw new Exception("Unable to insert row");

                        if (newRowIds != null)
                            newRowIds[i] = Convert.ToInt64(newRow);
                    }
                    else
                    {
                        if (await _command.ExecuteNonQueryAsync(cancellationToken) != 1)
                            throw new Exception("Unable to insert row");
                    }

                    if (_context.Debug.HasFlag(Context.DebugLevel.Inserts))
                        Console.Out.WriteLine("Insert: \r\n" + _command.CommandText);
                }

                // Commit all inserts.
                transaction?.Commit();
            }
            catch (Exception)
            {
                // If we encountered an error, rollback the transaction.
                transaction?.Rollback();

                throw;
            }
            finally
            {
                transaction?.Dispose();
            }

            if (_context.Debug.HasFlag(Context.DebugLevel.Inserts))
                Console.Out.WriteLine("Insert new Row IDs: \r\n" + string.Join(", ", newRowIds));

            return newRowIds;
        }


        /// <summary>
        /// Binds the specified parameters to the partial SQL statement.
        /// </summary>
        /// <param name="sql">SQL to bind the parameters to.</param>
        /// <param name="binding">Objects to bind to the partial SQL statement.</param>
        /// <returns>Formatted SQL string to put into the final SQL query.</returns>
        private string SqlBindParameters(string sql, object[] binding)
        {
            if (binding == null)
                return sql;

            var sqlParamHolder = new object[binding.Length];
            for (var i = 0; i < binding.Length; i++)
                sqlParamHolder[i] = BindParameter(binding[i]);

            try
            {
                return string.Format(sql, sqlParamHolder);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid number of placement parameters for the WHERE statement.", e);
            }
        }

        /// <summary>
        /// Validates the current state of the class and checks to see if a where statement is allowed to be called.
        /// </summary>
        private void ValidateWhere()
        {
            switch (_mode)
            {
                case Mode.Execute:
                    throw new InvalidOperationException("Can not use all functions in Execute mode.");
                case Mode.Insert:
                    throw new InvalidOperationException("Can not use the WHERE method in INSERT mode.");
            }

            if (_sqlWhere != null)
                throw new InvalidOperationException("The WHERE statement has already been defined.");
        }
    }
}