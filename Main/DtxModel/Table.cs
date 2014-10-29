using DtxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace DtxModel {
	public class Table<T> where T : Model, new() {
		private Context context;

		public SqlStatement<T> Select(string select = "*") {
			try {
				return new SqlStatement<T>(SqlStatement<T>.Mode.Select, context).Select(select);
			} catch (Exception e) {
				throw new InvalidOperationException("SQL operations are not allowed outside of the Database Context.", e);
			}
		}

		public void Insert(T model) {
			Insert(new T[] { model });
		}

		public void Insert(T[] model) {
			try {
				new SqlStatement<T>(SqlStatement<T>.Mode.Insert, context).Insert(model);
			} catch (Exception e) {
				throw new InvalidOperationException("SQL operations are not allowed outside of the Database Context.", e);
			}
		}

		public void Update(T model) {
			Update(new T[] { model });
		}

		public void Update(T[] model) {
			try {
				new SqlStatement<T>(SqlStatement<T>.Mode.Update, context).Update(model);
			} catch (Exception e) {
				throw new InvalidOperationException("SQL operations are not allowed outside of the Database Context.", e);
			}
		}

		public void Delete(T model) {
			Delete(new T[] { model });
		}

		public void Delete(T[] models) {
			try {
				new SqlStatement<T>(SqlStatement<T>.Mode.Delete, context).Delete(models);
			} catch (Exception e) {
				throw new InvalidOperationException("SQL operations are not allowed outside of the Database Context.", e);
			}
		}

		public Table(Context context) {
			this.context = context;
		}
	}
}
