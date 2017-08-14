﻿using System;
using System.Data.Common;

namespace DtronixModel
{
    /// <summary>
    /// Transaction wrapper to handle internal transactions and object states.
    /// </summary>
    public class DtronixTransaction : IDisposable
    {
        /// <summary>
        /// Method to call on transaction disposal.
        /// </summary>
        private readonly Action _onDispose;

        /// <summary>
        /// Wrapped transaction for the database.
        /// </summary>
        private readonly DbTransaction _transaction;

        private bool _disposed;

        /// <summary>
        /// Creates a wrapped transaction with action on disposal.
        /// </summary>
        /// <param name="transaction">Transaction to wrap.</param>
        /// <param name="onDispose">Method to call on transaction disposal.</param>
        public DtronixTransaction(DbTransaction transaction, Action onDispose)
        {
            _transaction = transaction;
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
            _transaction.Commit();
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void Rollback()
        {
            _transaction.Rollback();
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
                _transaction.Dispose();
                _onDispose();
            }

            _disposed = true;
        }
    }
}