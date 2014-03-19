using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace DtxModel {

	public class SqlStatement<T> : IDisposable where T : Model, new() {
		public enum Mode {
			Execute,
			Select,
			Insert,
			Update,
			Delete
		}

		private bool _auto_close_command = true;

		public bool AutoCloseCommand {
			get { return _auto_close_command; }
			set { _auto_close_command = value; }
		}


		public DbConnection connection;
		public DbCommand command;

		private Mode mode;

		private string table_name;

		private string sql_select = "*";
		private int sql_limit_start = -1;
		private int sql_limit_count = -1;
		private string sql_where;
		private Dictionary<string, SortDirection> sql_orders;
		private List<string> sql_groups;
		private T[] sql_update_models;

		//private 

		//public SqlStatement(Mode mode) : this(mode, null) { }

		public SqlStatement(Mode mode, DbConnection connection) {
			this.connection = connection;
			this.mode = mode;
			command = connection.CreateCommand();

			try {
				table_name = AttributeCache<T, TableAttribute>.getAttribute().Name;
			} catch (Exception) {
				throw new Exception("Class passed does not have a TableAttribute");
			}

		}

		/// <summary>
		/// Executes a string on the specified database.
		/// Will close the command after execution.
		/// </summary>
		/// <param name="sql">SQL to execute with parameters in string.format style.</param>
		/// <param name="binding">Parameters to replace the string.format placeholders with.</param>
		/// <returns>The number of rows affected.</returns>
		public int executeSql(string sql, params object[] binding) {
			return executeSql(sql, true, binding);
		}

		/// <summary>
		/// Executes a string on the specified database.
		/// </summary>
		/// <param name="sql">SQL to execute with parameters in string.format style.</param>
		/// <param name="close_command">If true, command will be disposed after execution.</param>
		/// <param name="binding">Parameters to replace the string.format placeholders with.</param>
		/// <returns>The number of rows affected.</returns>
		public int executeSql(string sql, bool close_command, params object[] binding) {
			if (mode != Mode.Execute) {
				throw new InvalidOperationException("Need to be in Execute mode to use this method.");
			}
			command.Parameters.Clear();
			command.CommandText = sqlBindParameters(sql, binding);
			int result = command.ExecuteNonQuery();

			if (close_command) {
				command.Dispose();
			}

			command.ExecuteReader().clo

			return result;
		}

		private string sqlBindParameters(string sql, object[] binding){
			string[] sql_param_holder = new string[binding.Length];
			for (int i = 0; i < binding.Length; i++) {
				sql_param_holder[i] = bindParameter(binding[i]);
			}

			try {
				return string.Format(sql, sql_param_holder);
			} catch (Exception e) {
				throw new Exception("Invalid number of placement parameters for the WHERE statement.", e);
			}
		}

		public SqlStatement<T> select(string select) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			this.sql_select = select;
			return this;
		}

		public void update(T model) {
			update(new T[] { model });
		}

		public void update(T[] model) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			sql_update_models = model;
			execute();
		}

		/// <summary>
		/// Sets where to the provided model's primary key.
		/// </summary>
		/// <param name="model">Model to provide the primary key for.</param>
		/// <returns>Current statement for chaining.</returns>
		public SqlStatement<T> where(T model) {
			// Set the update by the primary key.
			string primary_key;
			object primary_key_value;

			model.getPrimaryKey(out primary_key, out primary_key_value);
			where(primary_key + " == {0}", primary_key_value);

			return this;
		}


		public SqlStatement<T> where(string where, params object[] parameters) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode == Mode.Insert) {
				throw new InvalidOperationException("Can not use the WHERE method in INSERT mode.");
			}

			if (sql_where != null) {
				throw new InvalidOperationException("The WHERE statement has already been defined.");
			}

			sql_where = sqlBindParameters(where, parameters);

			return this;
		}

		private void resetState() {
			sql_groups = null;
			sql_limit_count = -1;
			sql_limit_start = -1;
			sql_orders = null;
			sql_select = null;
			sql_where = null;
			command.Parameters.Clear();
		}

		public SqlStatement<T> limit(int count) {
			return limit(count, -1);
		}

		public SqlStatement<T> limit(int count, int start) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not use the LIMIT method except in SELECT mode.");
			}

			sql_limit_start = start;
			sql_limit_count = count;

			return this;
		}

		public SqlStatement<T> orderBy(string column, SortDirection direction) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not use the ORDER BY method except in SELECT mode.");
			}

			if (sql_orders == null) {
				sql_orders = new Dictionary<string, SortDirection>();
			}

			sql_orders.Add(column, direction);

			return this;
		}

		public SqlStatement<T> groupBy(string column) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not use the GROUP BY method except in SELECT mode.");
			}

			if (sql_groups == null) {
				sql_groups = new List<string>();
			}

			sql_groups.Add(column);

			return this;
		}

		public void execute() {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode == Mode.Update) {
				using (var transaction = connection.BeginTransaction()) {

					for (int i = 0; i < sql_update_models.Length; i++) {
						resetState();
						where(sql_update_models[i]);
						buildSql(sql_update_models[i]);

						// Execute the update command.
						command.ExecuteNonQuery();
					}

					transaction.Commit();
				}

			} else {
				buildSql(null);
				command.ExecuteNonQuery();
			}

			if (_auto_close_command) {
				command.Dispose();
			}

		}


		public T executeFetch() {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not fetch from the server when not in SELECT mode.");
			}

			buildSql(null);
			T model;

			using (var reader = command.ExecuteReader()) {
				if (reader.Read() == false) {
					return default(T);
				}

				model = new T();
				model.read(reader, connection);
			}

			if (_auto_close_command) {
				command.Dispose();
			}

			return model;
		}

		public T[] executeFetchAll() {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not fetch from the server when not in SELECT mode.");
			}

			buildSql(null);

			var results = new List<T>();
			using (var reader = command.ExecuteReader()) {
				while (reader.Read()) {
					T model = new T();
					model.read(reader, connection);
					results.Add(model);
				}
			}

			if (_auto_close_command) {
				command.Dispose();
			}

			return results.ToArray();
		}

		private void buildSql(T model) {
			var sql = new StringBuilder();

			switch (mode) {
				case Mode.Select:
					sql.Append("SELECT ").AppendLine(sql_select);
					sql.Append("FROM ").AppendLine(table_name);
					break;
				case Mode.Insert:
					throw new InvalidOperationException("Can not build an SQL query in the INSERT mode.");
				case Mode.Update:
					sql.Append("UPDATE ").AppendLine(table_name);
					sql.Append("SET ");

					var changed_fields = model.getChangedValues();

					// If there are no fields to update, then do nothing.
					if (changed_fields.Count == 0) {
						sql.Clear();
					}

					foreach (var field in changed_fields) {
						sql.Append(field.Key).Append(" = ").Append(bindParameter(field.Value)).Append(", ");
					}

					sql.Remove(sql.Length - 2, 2).AppendLine();
					break;
				case Mode.Delete:
					sql.Append("DELETE");
					break;

				case Mode.Execute:
					throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}



			// WHERE
			if (mode != Mode.Insert && sql_where != null) {
				sql.Append("WHERE ").AppendLine(sql_where);
			}

			// GROUP BY
			if (mode == Mode.Select && sql_groups != null) {
				sql.Append("GROUP BY ");
				foreach (var group_column in sql_groups) {
					sql.Append(group_column).Append(", ");
				}
				sql.Remove(sql.Length - 2, 2).AppendLine();
			}

			// ORDER BY
			if (mode == Mode.Select && sql_orders != null) {
				sql.Append("ORDER BY ");
				foreach (var order_column in sql_orders.Keys) {
					sql.Append(order_column);

					switch (sql_orders[order_column]) {
						case SortDirection.Ascending:
							sql.Append(" ASC, ");
							break;
						case SortDirection.Descending:
							sql.Append(" DESC, ");
							break;
					}
				}

				// Remove the trailing ", "
				sql.Remove(sql.Length - 2, 2).AppendLine();
			}

			if (mode == Mode.Select && sql_limit_count != -1) {
				sql.Append("LIMIT ");

				if (sql_limit_start != -1) {
					sql.Append(sql_limit_start).Append(", ");
				}

				sql.Append(sql_limit_count);

			}

			command.CommandText = sql.ToString();
		}


		/// <summary>
		/// Binds a parameter in the current command.
		/// </summary>
		/// <param name="value">Value to bind.</param>
		/// <returns>Parameter name for the binding reference.</returns>
		private string bindParameter(object value, List<DbParameter> parameter_list = null) {
			string key = "@p" + command.Parameters.Count;
			var param = command.CreateParameter();
			param.ParameterName = key;
			param.Value = value;

			if (parameter_list != null) {
				parameter_list.Add(param);
			}

			command.Parameters.Add(param);
			return key;
		}

		/// <summary>
		/// Inserts a model into the database.
		/// </summary>
		/// <param name="models">Model to insert.</param>
		public void insert(T models) {
			insert(new T[] { models });
		}

		/// <summary>
		/// Insert multiple models into the database.
		/// </summary>
		/// <remarks>
		/// This method by default wraps all inserts into a transaction.
		/// If one of the inserts fails, then all of the inserts are rolled back.
		/// </remarks>
		/// <param name="models">Models to insert.</param>
		public void insert(T[] models) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (models == null || models.Length == 0) {
				throw new ArgumentException("Model array is empty.");
			}

			if (mode != Mode.Insert) {
				throw new InvalidOperationException("Can not insert when statement is not in INSERT mode.");
			}

			var columns = models[0].getColumns();

			StringBuilder sb_sql = new StringBuilder();
			sb_sql.Append("INSERT INTO ").Append(table_name).Append(" (");

			// Add all the column names.
			foreach (var column in columns) {
				sb_sql.Append("'").Append(column).Append("', ");
			}

			// Remove the last ", " from the query.
			sb_sql.Remove(sb_sql.Length - 2, 2);

			// Add the values.
			sb_sql.Append(") VALUES (");
			for (int i = 0; i < columns.Length; i++) {
				sb_sql.Append("@v").Append(i).Append(", ");
			}

			// Remove the last ", " from the query.
			sb_sql.Remove(sb_sql.Length - 2, 2);
			sb_sql.Append(")");


			// Start a transaction to enable for fast bulk inserts.
			using (var transaction = connection.BeginTransaction()) {

				try {

					command.CommandText = sb_sql.ToString();

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

				} catch (Exception e) {
					// If we incountered an error, rollback the transaction.
					transaction.Rollback();

					throw e;
				}

				// Commit all inserts.
				transaction.Commit();
			}
		}

		public void Dispose() {
			this.command.Dispose();
		}
	}
}
