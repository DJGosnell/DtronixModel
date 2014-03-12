using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;

namespace DtxModel {
	public class SqlStatement {
		public enum Mode {
			Select,
			Insert,
			Update,
			Delete
		}

		public DbConnection connection;

		public static SqlStatement Select () {
			return new SqlStatement(Mode.Select);
		}

		public static SqlStatement Update {
			get {
				return new SqlStatement(Mode.Update);
			}
		}

		public static SqlStatement Insert {
			get {
				return new SqlStatement(Mode.Insert);
			}
		}
        
		public static SqlStatement Delete {
			get {
				return new SqlStatement(Mode.Delete);
			}
		}


		private Mode mode;

		public SqlStatement(Mode mode) : this(mode, null) { }

		public SqlStatement(Mode mode, DbConnection connection) {
			this.connection = connection;
			this.mode = mode;
		}

		public void insert(string table, Model models) {
			insert(table, new Model[] { models });
		}

		public void insert(string table, Model[] models){
			if (models == null || models.Length == 0) {
				throw new ArgumentException("Model array is empty.");
			}

			int i = 0;
			var sql = buildInsertStatement(table, models[0].getColumns());

			using (var command = connection.CreateCommand()) {
				
				command.CommandText = sql;

				foreach(var model in models){
					var values = model.getAllValues();
					command.Parameters.Clear();
					i = 0;

					foreach(var param_value in values){
						var parameter = command.CreateParameter();
						parameter.ParameterName = "@v" + i++;
						parameter.Value = param_value.Value;
					
						command.Parameters.Add(parameter);
					}

					if (command.ExecuteNonQuery() != 1) {
						throw new Exception("Unable to insert row");
					}
				}
			}
		}

		/*public long insert(string table, Model[] model) {


			Dictionary<string, object> values = model[0].getAllValues();
			var sql = buildInsertStatement(table, values);
			long last_rowid = -1;

			using (var command = connection.CreateCommand()) {
				int i = 0;
				command.CommandText = sql;

				Dictionary<string, object> values = model.getAllValues();
				foreach (var param_value in values) {
					var parameter = command.CreateParameter();
					parameter.ParameterName = "@v" + i++;
					parameter.Value = param_value.Value;

					command.Parameters.Add(parameter);
				}
				command.ExecuteNonQuery();

				// Get the last added rowid.
				command.CommandText = @"select last_insert_rowid()";
				last_rowid = (long)command.ExecuteScalar();
			}

			return last_rowid;
		}*/

		/// <summary>
		/// Builds the SQL insertion query.
		/// </summary>
		/// <returns>Insert query for this single insertion transaction.</returns>
		private string buildInsertStatement(string table, string[] columns) {
			StringBuilder sql = new StringBuilder();
			sql.Append("INSERT INTO ").Append(table).Append(" (");

			// Add all the column names.
			foreach (var column in columns) {
				sql.Append("'").Append(column).Append("', ");
			}

			// Remove the last ", " from the query.
			sql.Remove(sql.Length - 2, 2);

			// Add the values.
			sql.Append(") VALUES (");
			for (int i = 0; i < columns.Length; i++) {
				sql.Append("@v").Append(i).Append(", ");
			}

			// Remove the last ", " from the query.
			sql.Remove(sql.Length - 2, 2);
			sql.Append(")");

			return sql.ToString();
		}
	}
}
