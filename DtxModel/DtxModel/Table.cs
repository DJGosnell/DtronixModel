using DtxModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace DtxModelTests.Northwind.Models {
	public class Table<T> where T : Model {
		private DbConnection connection;
		protected readonly string table_name;


		public SqlStatement<T> select() {
			return new SqlStatement<T>(SqlStatement<T>.Mode.Select, connection);
		}

		public void insert(T model) {
			new SqlStatement<T>(SqlStatement<T>.Mode.Insert, connection).insert(table_name, model);
		}

		public void insert(T[] model) {
			new SqlStatement<T>(SqlStatement<T>.Mode.Insert, connection).insert(table_name, model);
		}



		public SqlStatement<T> update() {
			return new SqlStatement<T>(SqlStatement<T>.Mode.Update, connection);
		}

		public SqlStatement<T> delete() {
			return new SqlStatement<T>(SqlStatement<T>.Mode.Delete, connection);
		}

		public Table(DbConnection connection, string table_name) {
			this.connection = connection;
			this.table_name = table_name;
		}
	}
}
