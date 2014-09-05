using DtxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace DtxModel {
	public class Table<T> where T : Model, new() {
		private Context context;


		public SqlStatement<T> select(string select = "rowid, *") {
			try {
				return new SqlStatement<T>(SqlStatement<T>.Mode.Select, context).select(select);
			} catch (Exception e) {
				throw new InvalidOperationException("SQL operations are not allowed outside of the Database Context.", e);
			}
		}

		public void insert(T model) {
			insert(new T[] { model });
		}

		public void insert(T[] model) {
			try {
				new SqlStatement<T>(SqlStatement<T>.Mode.Insert, context).insert(model);
			} catch (Exception e) {
				throw new InvalidOperationException("SQL operations are not allowed outside of the Database Context.", e);
			}
		}

		public void update(T model) {
			update(new T[] { model });
		}

		public void update(T[] model) {
			try {
				new SqlStatement<T>(SqlStatement<T>.Mode.Update, context).update(model);
			} catch (Exception e) {
				throw new InvalidOperationException("SQL operations are not allowed outside of the Database Context.", e);
			}
		}

		public void delete(T model) {
			delete(new T[] { model });
		}

		public void delete(T[] models) {
			try {
				new SqlStatement<T>(SqlStatement<T>.Mode.Delete, context).delete(models);
			} catch (Exception e) {
				throw new InvalidOperationException("SQL operations are not allowed outside of the Database Context.", e);
			}
		}

		public Table(Context context) {
			this.context = context;
		}
	}
}
