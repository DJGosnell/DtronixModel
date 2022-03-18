using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DtronixModel
{
    public partial class SqlStatement<T> : IDisposable
        where T : TableRow, new()
    {

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
        /// Executes a string on the specified database and calls calls method with the reader.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <returns>The reader for this query.</returns>
        public SqlStatementReader<T> QueryRead(string sql, object[] binding)
        {
            if (_mode != Mode.Execute)
                throw new InvalidOperationException("Need to be in Execute mode to use this method.");

            // Open the connection.
            _context.Open();

            Command.Parameters.Clear();
            Command.CommandText = SqlBindParameters(sql, binding);

            _logger?.Debug("Query: \r\n" + Command.CommandText);

            return new SqlStatementReader<T>(this, Command.ExecuteReader());
        }


        /// <summary>
        /// Executes a string on the specified database.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> QueryAsync(
            string sql, 
            object[] binding,
            CancellationToken cancellationToken = default)
        {
            if (_mode != Mode.Execute)
                throw new InvalidOperationException("Need to be in Execute mode to use this method.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            Command.Parameters.Clear();
            Command.CommandText = SqlBindParameters(sql, binding);

            _logger?.Debug("Query: \r\n" + Command.CommandText);

            var result = await Command.ExecuteNonQueryAsync(cancellationToken);

            Command.Dispose();

            return result;
        }

        /// <summary>
        /// Executes a string on the specified database and calls calls method with the reader.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<SqlStatementReader<T>> QueryReadAsync(
            string sql,
            object[] binding,
            CancellationToken cancellationToken = default)
        {
            if (_mode != Mode.Execute)
                throw new InvalidOperationException("Need to be in Execute mode to use this method.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            Command.Parameters.Clear();
            Command.CommandText = SqlBindParameters(sql, binding);

            _logger?.Debug("Query: \r\n" + Command.CommandText);

            return new SqlStatementReader<T>(this, await Command.ExecuteReaderAsync(cancellationToken));
        }

        /// <summary>
        /// Executes a string on the specified database and calls calls method with the reader.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<SqlStatementReader<T>> QueryReadAsync(
            string sql,
            CancellationToken cancellationToken = default)
        {
            if (_mode != Mode.Execute)
                throw new InvalidOperationException("Need to be in Execute mode to use this method.");

            // Open the connection.
            await _context.OpenAsync(cancellationToken);

            Command.CommandText = sql;

            _logger?.Debug("Query: \r\n" + Command.CommandText);

            return new SqlStatementReader<T>(this, await Command.ExecuteReaderAsync(cancellationToken));
        }

        /// <summary>
        /// Executes a string on the specified database and calls calls method with the reader.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <param name="onRead">Called when the query has been executed and reader created.</param>
        [Obsolete("Use QueryRead(string sql, object[] binding) instead.")]
        public void QueryRead(string sql, object[] binding, Action<DbDataReader> onRead)
        {
            using var reader = QueryRead(sql, binding);
            onRead(reader.Reader);
        }

        /// <summary>
        /// Executes a string on the specified database and calls calls method with the reader.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="binding">Parameters to replace the string.format placeholders with.</param>
        /// <param name="onRead">Called when the query has been executed and reader created.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [Obsolete("Use QueryReadAsync(string sql, object[] binding, CancellationToken cancellationToken) instead.")]
        public async Task QueryReadAsync(
            string sql,
            object[] binding,
            Func<DbDataReader, CancellationToken, Task> onRead,
            CancellationToken cancellationToken = default)
        {
            await using var reader = await QueryReadAsync(sql, binding, cancellationToken);
            await onRead(reader.Reader, cancellationToken);
        }

        /// <summary>
        /// Executes a string on the specified database and calls calls method with the reader.
        /// Will close the command after execution.
        /// </summary>
        /// <param name="sql">SQL to execute with parameters in string.format style.</param>
        /// <param name="onRead">Called when the query has been executed and reader created.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        [Obsolete("Use QueryReadAsync(string sql, CancellationToken cancellationToken) instead.")]
        public async Task QueryReadAsync(
            string sql,
            Func<DbDataReader, CancellationToken, Task> onRead,
            CancellationToken cancellationToken = default)
        {
            await using var reader = await QueryReadAsync(sql, cancellationToken);
            await onRead(reader.Reader, cancellationToken);
        }
    }
}
