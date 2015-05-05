using System;
using System.Data.Common;

namespace DtxModel {
	/// <summary>
	/// Transaction wrapper to handle internal transactions and object states.
	/// </summary>
	public class DtxTransaction : IDisposable {
		bool disposed = false;

		/// <summary>
		/// Wrapped transaction for the database.
		/// </summary>
		DbTransaction transaction;

		/// <summary>
		/// Method to call on transaction disposal.
		/// </summary>
		Action on_dispose;

		/// <summary>
		/// Creates a wrapped transaction with action on disposal.
		/// </summary>
		/// <param name="transaction">Transaction to wrap.</param>
		/// <param name="on_dispose">Method to call on transaction disposal.</param>
		public DtxTransaction(DbTransaction transaction, Action on_dispose) {
			this.transaction = transaction;
			this.on_dispose = on_dispose;
		}

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		public void Commit() {
			transaction.Commit();
		}

		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		public void Rollback() {
			transaction.Rollback();
		}

		/// <summary>
		/// Releases the unmanaged resources used by the System.Data.Common.DbTransaction.
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Protected implementation of Dispose pattern. 
		protected virtual void Dispose(bool disposing) {
			if (disposed)
				return;

			if (disposing) {
				transaction.Dispose();
				on_dispose();
			}

			disposed = true;
		}
	}
}
