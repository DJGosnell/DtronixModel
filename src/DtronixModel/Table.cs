using System;
using System.Threading;
using System.Threading.Tasks;
using DtronixModel.Attributes;

namespace DtronixModel
{
    /// <summary>
    /// Generic class to wrap individual table models.
    /// </summary>
    /// <typeparam name="T">Table model to wrap.</typeparam>
    public class Table<T>
        where T : TableRow, new()
    {
        private readonly Context _context;

        /// <summary>
        /// Constructor to define which context this table is operating inside.
        /// </summary>
        /// <param name="context">Context of the current database.</param>
        public Table(Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Begins a select query. Ensure to dispose of SqlStatement
        /// </summary>
        /// <param name="select"></param>
        /// <returns>SqlStatement to chain up following query modifiers.</returns>
        public SqlStatement<T> Select(string select = "*")
        {
            return new SqlStatement<T>(SqlStatement<T>.Mode.Select, _context).Select(select);
        }

        /// <summary>
        /// Begins a select query. Ensure to dispose of SqlStatement
        /// </summary>
        /// <param name="select"></param>
        /// <returns>SqlStatement to chain up following query modifiers.</returns>
        public SqlStatement<T> Select(params string[] select)
        {
            return new SqlStatement<T>(SqlStatement<T>.Mode.Select, _context).Select(select);
        }

        /// <summary>
        /// Fetches a the first row matching the specified column's value asynchronously.
        /// </summary>
        /// <param name="column">Column to lookup the specified value on.</param>
        /// <param name="value">Value to lookup on the specified column.</param>
        /// <returns>Row matching the specified parameters.</returns>
        public Task<T> FetchByColumnValue(string column, object value)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Select, _context)
                .Select("*")
                .Where($"{column} = {{0}}", value)
                .Limit(1);

            return statement.ExecuteFetchAsync();
        }

        /// <summary>
        /// Fetches a the first row matching the specified column's value asynchronously.
        /// </summary>
        /// <param name="column">Column to lookup the specified value on.</param>
        /// <param name="value">Value to lookup on the specified column.</param>
        /// <param name="count">Number of rows to return. -1 for unlimited rows.</param>
        /// <param name="offset">Number of rows offset the counter into the return set.</param>
        /// <returns>Row matching the specified parameters if found.  Empty array otherwise.</returns>
        public Task<T[]> FetchAllByColumnValue(string column, object value, int count = -1, int offset = 0)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (count < -1 || count == 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Select, _context)
                .Select("*")
                .Where($"{column} = {{0}}", value);

            if (count > 0)
                statement.Limit(count, offset);

            return statement.ExecuteFetchAllAsync();
        }

        /// <summary>
        /// Asynchronously fetches row by the specified primary key's value.
        /// </summary>
        /// <param name="value">Primary key value to lookup.</param>
        /// <returns>Row matching with the specified primary key.</returns>
        public Task<T> FetchByPk(object value)
        {
            var pkName = AttributeCache<T, TableAttribute>.GetAttribute().PrimaryKey;

            if (pkName == null)
                throw new Exception("Table does not have a set primary key");

            return FetchByColumnValue(pkName, value);
        }

        /// <summary>
        /// Inserts a single row into the database.
        /// </summary>
        /// <param name="model">Row to insert.</param>
        /// <returns>
        /// If the "LastInsertIdQuery" property is set on the database context, will return the newly inserted row id.
        /// Otherwise will return 0.
        /// </returns>
        public long Insert(T model)
        {
            var result = Insert(new[] { model });
            return result?[0] ?? 0;
        }

        /// <summary>
        /// Inserts multiple rows into the database inside a transaction.
        /// </summary>
        /// <remarks>
        /// Since all of the inserts are performed inside a transaction, if one of the inserts fail, all of the inserts
        /// rollback.
        /// </remarks>
        /// <param name="model">Row to insert.</param>
        /// <returns>
        /// If the "LastInsertIdQuery" property is set on the database context, will return the newly inserted row ids in
        /// a long[].  Otherwise will return null.
        /// </returns>
        public long[] Insert(T[] model)
        {
            using var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Insert, _context);
            return statement.Insert(model);
        }

        /// <summary>
        /// Inserts a single row into the database asynchronously.
        /// </summary>
        /// <param name="model">Row to insert.</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>
        /// If the "LastInsertIdQuery" property is set on the database context, will return the newly inserted row id.
        /// Otherwise will return 0.
        /// </returns>
        public async Task<long> InsertAsync(T model, CancellationToken cancellationToken = default)
        {
            var result = await InsertAsync(new[] { model }, cancellationToken);
            return result?[0] ?? 0;
        }

        /// <summary>
        /// Inserts multiple rows into the database inside a transaction asynchronously.
        /// </summary>
        /// <remarks>
        /// Since all of the inserts are performed inside a transaction, if one of the inserts fail, all of the inserts
        /// rollback.
        /// </remarks>
        /// <param name="model">Row to insert.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>
        /// If the "LastInsertIdQuery" property is set on the database context, will return the newly inserted row ids in
        /// a long[].  Otherwise will return null.
        /// </returns>
        public async Task<long[]> InsertAsync(T[] model, CancellationToken cancellationToken = default)
        {
            await using var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Insert, _context);
            return await statement.InsertAsync(model, cancellationToken);
        }

        /// <summary>
        /// Updates a single row in the database. Must have its primary key set.
        /// </summary>
        /// <remarks>
        /// The primary key (Usually the row id) has to be set for the function to determine which row to update.
        /// </remarks>
        /// <param name="model">Row to update.</param>
        public void Update(T model)
        {
            Update(new[] { model });
        }

        /// <summary>
        /// Updates multiple rows in the database. Must have their primary keys set.
        /// </summary>
        /// <remarks>
        /// The primary key (Usually the row id) has to be set for the function to determine which row to update.
        /// </remarks>
        /// <param name="model">Rows to update.</param>
        public void Update(T[] model)
        {
            using var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Update, _context);
            statement.Update(model);
        }

        /// <summary>
        /// Updates a single row in the database asynchronously. Must have its primary key set.
        /// </summary>
        /// <remarks>
        /// The primary key (Usually the row id) has to be set for the function to determine which row to update.
        /// </remarks>
        /// <param name="model">Row to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task UpdateAsync(T model, CancellationToken cancellationToken = default)
        {
            await UpdateAsync(new[] { model }, cancellationToken);
        }

        /// <summary>
        /// Updates multiple rows in the database asynchronously. Must have their primary keys set.
        /// </summary>
        /// <remarks>
        /// The primary key (Usually the row id) has to be set for the function to determine which row to update.
        /// </remarks>
        /// <param name="models">Rows to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task UpdateAsync(T[] models, CancellationToken cancellationToken = default)
        {
            await using var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Update, _context);
            await statement.UpdateAsync(models, cancellationToken);
        }

        /// <summary>
        /// Deletes a row from the database. Must have its primary key set.
        /// </summary>
        /// <remarks>
        /// The primary key (Usually the row id) has to be set for the function to determine which row to update.
        /// </remarks>
        /// <param name="model">Row to delete.</param>
        public void Delete(T model)
        {
            Delete(new[] { model });
        }

        /// <summary>
        /// Deletes multiple rows in the database. Must have their primary keys set.
        /// </summary>
        /// <param name="models">Rows to delete.</param>
        public void Delete(T[] models)
        {
            using var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Delete, _context);
            statement.Delete(models);
        }

        /// <summary>
        /// Deletes a single row in the database based upon the primary row id.
        /// </summary>
        /// <param name="id">Row id to delete.</param>
        public void Delete(long id)
        {
            Delete(new[] { id });
        }

        /// <summary>
        /// Delete multiple rows in the database based upon the primary row ids.
        /// </summary>
        /// <param name="ids">Row ids to delete.</param>
        public void Delete(long[] ids)
        {
            using var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Delete, _context);
            statement.Delete(ids);
        }

        /// <summary>
        /// Deletes a row from the database asynchronously. Must have its primary key set.
        /// </summary>
        /// <remarks>
        /// The primary key (Usually the row id) has to be set for the function to determine which row to update.
        /// </remarks>
        /// <param name="model">Row to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task DeleteAsync(T model, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(new[] { model }, cancellationToken);
        }

        /// <summary>
        /// Deletes multiple rows in the database asynchronously. Must have their primary keys set
        /// </summary>
        /// <param name="models">Rows to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task DeleteAsync(T[] models, CancellationToken cancellationToken = default)
        {
            await using var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Delete, _context);
            await statement.DeleteAsync(models, cancellationToken);
        }

        /// <summary>
        /// Deletes a single row in the database based upon the primary row id asynchronously.
        /// </summary>
        /// <param name="id">Row id to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(new[] { id }, cancellationToken);
        }

        /// <summary>
        /// Delete multiple rows in the database based upon the primary row ids asynchronously.
        /// </summary>
        /// <param name="ids">Row ids to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task DeleteAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            await using var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Delete, _context);
            await statement.DeleteAsync(ids, cancellationToken);
        }
    }
}