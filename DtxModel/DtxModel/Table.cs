using DtxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace DtxModel {
	public class Table<T> where T : Model, new() {
		private DbConnection connection;


		public SqlStatement<T> select(string select = "*, rowid") {
			return new SqlStatement<T>(SqlStatement<T>.Mode.Select, connection).select(select);
		}

		public void insert(T model) {
			insert(new T[] { model });
		}

		public void insert(T[] model) {
			new SqlStatement<T>(SqlStatement<T>.Mode.Insert, connection).insert(model);
		}

		public void update(T model) {
			update(new T[] { model });
		}

		public void update(T[] model) {
			new SqlStatement<T>(SqlStatement<T>.Mode.Update, connection).update(model);
		}

		public void delete(T model) {
			delete(new T[] { model });
		}

		public void delete(T[] models) {
			new SqlStatement<T>(SqlStatement<T>.Mode.Delete, connection).delete(models);
		}

		public Table(DbConnection connection) {
			this.connection = connection;
		}
	}
}
