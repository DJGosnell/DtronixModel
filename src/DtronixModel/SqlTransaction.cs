using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace DtronixModel
{
    /// <summary>
    /// Transaction wrapper to handle internal transactions and object states.
    /// </summary>
    public sealed class SqlTransaction : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Method to call on transaction disposal.
        /// </summary>
        private readonly Action _onDispose;

        /// <summary>
        /// Wrapped transaction for the database.
        /// </summary>
        public DbTransaction Transaction { get; }

        private bool _disposed;

        /// <summary>
        /// Creates a wrapped transaction with action on disposal.
        /// </summary>
        /// <param name="transaction">Transaction to wrap.</param>
        /// <param name="onDispose">Method to call on transaction disposal.</param>
        public SqlTransaction(DbTransaction transaction, Action onDispose)
        {
            Transaction = transaction;
            _onDispose = onDispose;
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void Commit()
        {
            Transaction.Commit();
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return Transaction.CommitAsync(cancellationToken);
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void Rollback()
        {
            Transaction.Rollback();
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            return Transaction.RollbackAsync(cancellationToken);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the System.Data.Common.DbTransaction.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            Transaction.Dispose();
            _onDispose();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the System.Data.Common.DbTransaction.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;
            _disposed = true;

            await Transaction.DisposeAsync().ConfigureAwait(false);
            _onDispose();
        }
    }
}