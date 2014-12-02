using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtxModel {
	public class DtxTransaction : IDisposable {
		bool disposed = false;
		DbTransaction transaction;
		Action on_dispose;

		public DtxTransaction(DbTransaction transaction, Action on_dispose) {
			this.transaction = transaction;
			this.on_dispose = on_dispose;
		}

		public void Commit() {
			transaction.Commit();
		}

		public void Rollback() {
			transaction.Rollback();
		}

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
