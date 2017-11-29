using System;
using System.Data.Common;

namespace DtronixModel
{
    /// <summary>
    /// Transaction wrapper to handle internal transactions and object states.
    /// </summary>
    public class SqlTransaction : IDisposable
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
        /// Releases the unmanaged resources used by the System.Data.Common.DbTransaction.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void Commit()
        {
            Transaction.Commit();
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void Rollback()
        {
            Transaction.Rollback();
        }


        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">True if the object is in the process of being disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Transaction.Dispose();
                _onDispose();
            }

            _disposed = true;
        }
    }
}