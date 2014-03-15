using DtxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace DtxModelTests.Northwind.Models {
	public class Table<T> where T : Model, new() {
		private DbConnection connection;


		public SqlStatement<T> select(string select = "*, rowid") {
			return new SqlStatement<T>(SqlStatement<T>.Mode.Select, connection);
		}

		public void insert(T model) {
			new SqlStatement<T>(SqlStatement<T>.Mode.Insert, connection).insert(model);
		}

		public void insert(T[] model) {
			new SqlStatement<T>(SqlStatement<T>.Mode.Insert, connection).insert(model);
		}



		public SqlStatement<T> update() {
			return new SqlStatement<T>(SqlStatement<T>.Mode.Update, connection);
		}

		public SqlStatement<T> delete() {
			return new SqlStatement<T>(SqlStatement<T>.Mode.Delete, connection);
		}

		public Table(DbConnection connection) {
			this.connection = connection;
		}
	}
}
