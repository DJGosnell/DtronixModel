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


		public SqlStatement select() {
			return new SqlStatement(SqlStatement.Mode.Select, connection);
		}

		public void insert(T model) {
			new SqlStatement(SqlStatement.Mode.Insert, connection).insert(table_name, model);
		}

		public void insert(T[] model) {
			new SqlStatement(SqlStatement.Mode.Insert, connection).insert(table_name, model);
		}



		public SqlStatement update() {
			return new SqlStatement(SqlStatement.Mode.Update, connection);
		}

		public SqlStatement delete() {
			return new SqlStatement(SqlStatement.Mode.Delete, connection);
		}

		public Table(DbConnection connection, string table_name) {
			this.connection = connection;
			this.table_name = table_name;
		}
	}
}
