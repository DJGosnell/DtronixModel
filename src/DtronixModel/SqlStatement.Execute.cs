using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DtronixModel
{
    public partial class SqlStatement<T> : IDisposable
        where T : TableRow, new()
    {
        /// <summary>
        /// Executes the query built.
        /// </summary>
        public void Execute()
        {
            if (_mode == Mode.Execute)
                throw new InvalidOperationException("Can not use all functions in Execute mode.");

            // Open the connection.
            _context.Open();

            if (_mode == Mode.Update)
            {

                SqlTransaction transaction = null;
                try
                {
                    // Start a transaction if one does not already exist.
                    if (_context.Transaction == null && _sqlRows.Length > 1)
                        transaction = _context.BeginTransaction();

                    foreach (var model in _sqlRows)
                    {
                        _sqlWhere = null;
                        Where(model);
                        BuildSql(model);

                        // Execute the update command.
                        Command.ExecuteNonQuery();

                        _logger?.Debug("Update: \r\n" + Command.CommandText);
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
                Command.ExecuteNonQuery();
            }

            if (AutoCloseCommand)
                Command.Dispose();
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
                    if (_context.Transaction == null && _sqlRows.Length > 1)
                        transaction = await _context.BeginTransactionAsync(cancellationToken);

                    foreach (var model in _sqlRows)
                    {
                        _sqlWhere = null;
                        Where(model);
                        BuildSql(model);

                        // Execute the update command.
                        await Command.ExecuteNonQueryAsync(cancellationToken);

                        _logger?.Debug("Update: \r\n" + Command.CommandText);
                    }
                    if (transaction != null)
                        await transaction.CommitAsync(cancellationToken);
                }
                finally
                {
                    if(transaction != null)
                        await transaction.DisposeAsync();
                }
            }
            else
            {
                BuildSql(null);
                await Command.ExecuteNonQueryAsync(cancellationToken);
            }

            if (AutoCloseCommand)
                await Command.DisposeAsync();
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

            await using (var reader = await Command.ExecuteReaderAsync(cancellationToken))
            {
                if (await reader.ReadAsync(cancellationToken) == false)
                    return default;

                model = new T();
                model.Read(reader, _context);
            }

            if (AutoCloseCommand)
                Command.Dispose();

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
                Command.Parameters.Clear();
                Command.CommandText = SqlBindParameters(query, parameters);
            }

            var results = new List<T>();

            await using (var reader = await Command.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    var model = new T();
                    model.Read(reader, _context);
                    results.Add(model);
                }
            }

            if (AutoCloseCommand)
                Command.Dispose();

            return results.ToArray();
        }
    }
}
