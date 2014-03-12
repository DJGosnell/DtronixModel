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

		/// <summary>
		/// Inserts a model into the database.
		/// </summary>
		/// <param name="table">Name of the table to insert these models into.</param>
		/// <param name="models">Model to insert.</param>
		public void insert(string table, Model models) {
			insert(table, new Model[] { models });
		}

		/// <summary>
		/// Insert multiple models into the database.
		/// </summary>
		/// <remarks>
		/// This method by default wraps all inserts into a transaction.
		/// If one of the inserts fails, then all of the inserts are rolled back.
		/// </remarks>
		/// <param name="table">Name of the table to insert these models into.</param>
		/// <param name="models">Models to insert.</param>
		public void insert(string table, Model[] models){
			if (models == null || models.Length == 0) {
				throw new ArgumentException("Model array is empty.");
			}

			int i = 0;
			var sql = buildInsertStatement(table, models[0].getColumns());

			// Start a transaction to enable for fast bulk inserts.
			var transaction = connection.BeginTransaction();

			try {
				using (var command = connection.CreateCommand()) {
					command.CommandText = sql;

					// Loop through wach of the provided models.
					foreach (var model in models) {
						var values = model.getAllValues();
						command.Parameters.Clear();
						i = 0;

						foreach (var param_value in values) {
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
			} catch (Exception e) {
				// If we incountered an error, rollback the transaction.
				transaction.Rollback();
				throw e;
			}
			
			// Commit all inserts.
			transaction.Commit();
		}

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
