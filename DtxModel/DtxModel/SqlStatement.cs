using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Linq;
using System.Reflection;
using System.Data.Linq.Mapping;

namespace DtxModel {

	public class SqlStatement<T> {
		public enum Mode {
			Select,
			Insert,
			Update,
			Delete
		}


		public DbConnection connection;
		public DbCommand command;

		private Mode mode;
		private int param_index = 0;

		private string sql_select = "*, rowid";
		private int sql_limit_start;
		private int sql_limit_count;
		private string sql_where;
		private Dictionary<string, SortDirection> sql_order;
		private List<string> sql_group;

		//private 

		public SqlStatement(Mode mode) : this(mode, null) { }

		public SqlStatement(Mode mode, DbConnection connection) {
			this.connection = connection;
			this.mode = mode;
			command = connection.CreateCommand();

		}

		public SqlStatement<T> where(string where, params object[] parameters){
			if (mode == Mode.Insert) {
				throw new InvalidOperationException("Can not use the WHERE method in INSERT mode.");
			}

			if (sql_where != null) {
				throw new InvalidOperationException("The WHERE statement has already been defined.");
			}

			string[] sql_param_holder = new string[parameters.Length];
			for (int i = 0; i < parameters.Length; i++) {
				sql_param_holder[i] = bindParameter(parameters[i]);
			}

			try {
				sql_where = string.Format(where, sql_param_holder);
			} catch (Exception e) {
				throw new Exception("Invalid number of placement parameters for the WHERE statement.", e);
			}
			

			return this;
		}

		public SqlStatement<T> limit(int start, int count) {
			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not use the LIMIT method except in SELECT mode.");
			}

			sql_limit_start = start;
			sql_limit_count = count;

			return this;
		}

		public SqlStatement<T> orderBy(string column, SortDirection direction) {
			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not use the ORDER BY method except in SELECT mode.");
			}

			if (sql_order == null) {
				sql_order = new Dictionary<string, SortDirection>();
			}

			sql_order.Add(column, direction);

			return this;
		}

		public SqlStatement<T> groupBy(string column) {
			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not use the GROUP BY method except in SELECT mode.");
			}

			if (sql_group == null) {
				sql_group = new List<string>();
			}

			sql_group.Add(column);

			return this;
		}

		private void buildSql(){
			var sql = new StringBuilder();

			switch (mode) {
				case Mode.Select:
					sql.Append("SELECT");
					break;
				case Mode.Insert:
					break;
				case Mode.Update:
					sql.Append("UPDATE");
					break;
				case Mode.Delete:
					sql.Append("DELETE");
					break;
				default:
					break;
			}
				
		}


		private string bindParameter(object value){
			string key = "@p" + param_index++;
			var param = command.CreateParameter();
			param.ParameterName = key;
			param.Value = value;
			command.Parameters.Add(param);
			return key;
		}

		/// <summary>
		/// Inserts a model into the database.
		/// </summary>
		/// <param name="models">Model to insert.</param>
		public void insert(Model models) {
			insert(new Model[] { models });
		}

		/// <summary>
		/// Insert multiple models into the database.
		/// </summary>
		/// <remarks>
		/// This method by default wraps all inserts into a transaction.
		/// If one of the inserts fails, then all of the inserts are rolled back.
		/// </remarks>
		/// <param name="models">Models to insert.</param>
		public void insert(Model[] models){
			if (models == null || models.Length == 0) {
				throw new ArgumentException("Model array is empty.");
			}
			var columns = models[0].getColumns();

			string table_name;

			try {
				table_name = AttributeCache<T, TableAttribute>.getAttribute().Name;
			} catch (Exception) {
				throw new Exception("Class passed does not have a TableAttribute");
			}

			var sql = buildInsertStatement(table_name, columns);

			// Start a transaction to enable for fast bulk inserts.
			using (var transaction = connection.BeginTransaction()) {

				try {
					using (var command = connection.CreateCommand()) {
						command.CommandText = sql;

						// Create the parameters for bulk inerts.
						for (int i = 0; i < columns.Length; i++) {
							var parameter = command.CreateParameter();
							parameter.ParameterName = "@v" + i;
							command.Parameters.Add(parameter);
						}

						// Loop through wach of the provided models.
						foreach (var model in models) {
							var values = model.getAllValues();

							for (int i = 0; i < values.Length; i++) {
								command.Parameters[i].Value = values[i];
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
