using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace DtxModel {

	/// <summary>
	/// Class to help in quick and simple CRUD operations on a database table.
	/// </summary>
	/// <typeparam name="T">Model this class will be working with.</typeparam>
	public class SqlStatement<T> : IDisposable where T : Model, new() {

		/// <summary>
		/// Mode this statement will be bound to.
		/// </summary>
		public enum Mode {
			Execute,
			Select,
			Insert,
			Update,
			Delete
		}

		private bool _auto_close_command = true;

		/// <summary>
		/// True to close the command at the end of the query.
		/// </summary>
		public bool AutoCloseCommand {
			get { return _auto_close_command; }
			set { _auto_close_command = value; }
		}


		private Context context;
		private DbCommand command;

		/// <summary>
		/// Internal mode that the statement was setup with.
		/// </summary>
		private Mode mode;

		/// <summary>
		/// Name of the table this statement is querying.
		/// </summary>
		private string table_name;

		/// <summary>
		/// Holds the columns to select.
		/// </summary>
		private string sql_select = "*";

		/// <summary>
		/// Offset for the query limits. -1 for no offset.
		/// </summary>
		private int sql_limit_offset = -1;

		/// <summary>
		/// Limits the number of returned rows in the query. -1 for no limits.
		/// </summary>
		private int sql_limit_count = -1;

		/// <summary>
		/// Contains the bound where portion of the query.
		/// </summary>
		private string sql_where;

		/// <summary>
		/// Contains a dictionary list of the sort orders for this query.
		/// </summary>
		private Dictionary<string, SortDirection> sql_orders;

		/// <summary>
		/// List containing groupings for the query.
		/// </summary>
		private List<string> sql_groups;

		/// <summary>
		/// Model array of the current rows to be inserted, deleted or updated.
		/// </summary>
		private T[] sql_models;


		/// <summary>
		/// Starts a Statement in the specified mode of operation.
		/// </summary>
		/// <param name="mode">Mode that this query will operate in. Prevents invalid operations.</param>
		/// <param name="context">Context that this query will operate inside of.</param>
		public SqlStatement(Mode mode, Context context) {
			this.context = context;
			this.mode = mode;
			command = context.Connection.CreateCommand();

			try {
				table_name = AttributeCache<T, TableAttribute>.GetAttribute().Name;
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
		public int Query(string sql, params object[] binding) {
			if (mode != Mode.Execute) {
				throw new InvalidOperationException("Need to be in Execute mode to use this method.");
			}

			command.Parameters.Clear();
			command.CommandText = SqlBindParameters(sql, binding);

			// Logging to output queries to stdout.
			if (context.Debug.HasFlag(Context.DebugLevel.Queries)) {
				Console.Out.WriteLine("Query: \r\n" + command.CommandText);
			}

			int result = command.ExecuteNonQuery();

			command.Dispose();

			return result;
		}

		/// <summary>
		/// Executes a string on the specified database and calls calls method with the reader.
		/// Will close the command after execution.
		/// </summary>
		/// <param name="sql">SQL to execute with parameters in string.format style.</param>
		/// <param name="binding">Parameters to replace the string.format placeholders with.</param>
		/// <param name="on_read">Called when the query has been executed and reader created.</param>
		/// <returns>The number of rows affected.</returns>
		public void QueryRead(string sql, object[] binding, Action<DbDataReader> on_read) {
			if (mode != Mode.Execute) {
				throw new InvalidOperationException("Need to be in Execute mode to use this method.");
			}

			command.Parameters.Clear();
			command.CommandText = SqlBindParameters(sql, binding);

			// Logging to output queries to stdout.
			if (context.Debug.HasFlag(Context.DebugLevel.Queries)) {
				Console.Out.WriteLine("Query: \r\n" + command.CommandText);
			}

			using (var reader = command.ExecuteReader()) {
				on_read(reader);
			}

			command.Dispose();
		}


		/// <summary>
		/// Begins selection process and the specifies columns to return from the database.
		/// </summary>
		/// <param name="select">Columns to select.  Selecting "*" will select all the columns in the table</param>
		/// <returns>Current statement for chaining.</returns>
		public SqlStatement<T> Select(string select) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			this.sql_select = select;
			return this;
		}

		/// <summary>
		/// Updates the specified rows in the database. The rows must have their primary keys set.
		/// </summary>
		/// <param name="models">Rows to update with their new values.</param>
		public void Update(T[] models) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			sql_models = models;
			Execute();
			command.Dispose();
		}


		/// <summary>
		/// Deletes the specified rows from the database. The rows must have their primary keys set.
		/// </summary>
		/// <param name="models">Rows to delete.</param>
		public void Delete(T[] models) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			Where(models);

			sql_models = models;
			Execute();
		}

		/// <summary>
		/// Deletes the specified primary keys from the table.
		/// </summary>
		/// <param name="primary_ids">Ids to delete.</param>
		public void Delete(long[] primary_ids) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			WhereIn(new T().GetPKName(), primary_ids.Cast<object>().ToArray());
			Execute();
		}

		/// <summary>
		/// Specifies a column to match the specified values.
		/// </summary>
		/// <param name="column">Column to match against.</param>
		/// <param name="values">Values to check against the specified column.</param>
		/// <returns>Current statement for chaining.</returns>
		public SqlStatement<T> WhereIn(string column, params object[] values) {
			ValidateWhere();

			if (string.IsNullOrWhiteSpace(column)) {
				throw new ArgumentException("Column parameter can not be empty.");
			}

			StringBuilder sql = new StringBuilder();
			sql.Append(column).Append(" IN(");

			foreach (var value in values) {
				sql.Append(BindParameter(value)).Append(",");
			}
			sql.Remove(sql.Length - 1, 1).Append(")");

			sql_where = sql.ToString();

			return this;
		}

		/// <summary>
		/// Sets where to the provided rows's primary key.
		/// </summary>
		/// <param name="model">Row to provide the primary key for.</param>
		/// <returns>Current statement for chaining.</returns>
		public SqlStatement<T> Where(T model) {
			return Where(new T[] { model });
		}

		/// <summary>
		/// Sets where to the provided row's primary keys.
		/// </summary>
		/// <param name="models">Row to provide the primary key for.</param>
		/// <returns>Current statement for chaining.</returns>
		public SqlStatement<T> Where(T[] models) {
			ValidateWhere();

			// Set the update by the primary key.
			if (models == null || models.Length == 0) {
				throw new ArgumentException("Models parameter can not be null or empty.");
			}

			// Get the primary key for the first parameter 
			string pk_name = models[0].GetPKName();

			StringBuilder sql = new StringBuilder();
			sql.Append(pk_name).Append(" IN(");

			foreach (var model in models) {
				sql.Append(BindParameter(model.GetPKValue())).Append(",");
			}
			sql.Remove(sql.Length - 1, 1).Append(")");

			sql_where = sql.ToString();

			return this;
		}


		/// <summary>
		/// Specifies a custom where string to be applied to the query. Use the String.Format type arguments for this method.
		/// </summary>
		/// <param name="where">Where string to apply to the query. Use String.Format holders ({0}, {1}, etc...) for the bound parameters.</param>
		/// <param name="parameters">Parameters to bind to the query.</param>
		/// <returns>Current statement for chaining.</returns>
		public SqlStatement<T> Where(string where, params object[] parameters) {
			ValidateWhere();

			sql_where = SqlBindParameters(where, parameters);

			return this;
		}

		/// <summary>
		/// Limits the rows returned by the server.
		/// </summary>
		/// <param name="count">Number of rows to return.</param>
		/// <returns>Current statement for chaining.</returns>
		public SqlStatement<T> Limit(int count) {
			return Limit(count, -1);
		}

		/// <summary>
		/// Limits the rows returned by the server.
		/// </summary>
		/// <param name="count">Number of rows to return.</param>
		/// <param name="offset">Number of rows offset the counter into the return set.</param>
		/// <returns>Current statement for chaining.</returns>
		public SqlStatement<T> Limit(int count, int offset) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not use the LIMIT method except in SELECT mode.");
			}

			sql_limit_offset = offset;
			sql_limit_count = count;

			return this;
		}

		/// <summary>
		/// Orders the found results by the specified column.  Call multiple times to specify multiple orders.
		/// </summary>
		/// <param name="column">Column to order.</param>
		/// <param name="direction">Direction to order the specified column.</param>
		/// <returns>Current statement for chaining.</returns>
		public SqlStatement<T> OrderBy(string column, SortDirection direction) {
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

		/// <summary>
		/// Groups the statement by the specified column.  Call multiple times to specify multiple groups.
		/// </summary>
		/// <param name="column">Column to add to the group statement.</param>
		/// <returns>Current statement for chaining.</returns>
		public SqlStatement<T> GroupBy(string column) {
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

		/// <summary>
		/// Executes the query built.
		/// </summary>
		public void Execute() {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode == Mode.Update) {
				DtxTransaction transaction = null;
				try {
					// Start a transaction if one does not already exist.
					if (context.TransactionStarted == false) {
						transaction = context.BeginTransaction();
					}

					for (int i = 0; i < sql_models.Length; i++) {
						sql_where = null;
						Where(sql_models[i]);
						BuildSql(sql_models[i]);

						// Execute the update command.
						command.ExecuteNonQuery();
					}

					if (transaction != null) {
						transaction.Commit();
					}
				} finally {
					if (transaction != null) {
						transaction.Dispose();
					}
				}

			} else {
				BuildSql(null);
				command.ExecuteNonQuery();
			}

			if (_auto_close_command) {
				command.Dispose();
			}

		}

		/// <summary>
		/// Executes the query built and returns the associated rows with the query. Synchronous.
		/// </summary>
		/// <returns>On success, returns rows with the result of the query; Otherwise returns null.</returns>
		public T ExecuteFetch() {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not fetch from the server when not in SELECT mode.");
			}

			BuildSql(null);
			T model;

			using (var reader = command.ExecuteReader()) {
				if (reader.Read() == false) {
					return default(T);
				}

				model = new T();
				model.Read(reader, context);
			}

			if (_auto_close_command) {
				command.Dispose();
			}

			return model;
		}

		/// <summary>
		/// Executes the query built and returns the associated rows with the query. Synchronous.
		/// </summary>
		/// <returns>On success, returns rows with the result of the query; Otherwise returns an empty array.</returns>
		public T[] ExecuteFetchAll() {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode != Mode.Select) {
				throw new InvalidOperationException("Can not fetch from the server when not in SELECT mode.");
			}

			BuildSql(null);

			var results = new List<T>();
			using (var reader = command.ExecuteReader()) {
				while (reader.Read()) {
					T model = new T();
					model.Read(reader, context);
					results.Add(model);
				}
			}

			if (_auto_close_command) {
				command.Dispose();
			}

			return results.ToArray();
		}

		/// <summary>
		/// Builds the SQL statement that this class currently represents.
		/// </summary>
		/// <param name="model">Model to base this query on.</param>
		private void BuildSql(T model) {
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

					var changed_fields = model.GetChangedValues();

					// If there are no fields to update, then do nothing.
					if (changed_fields.Count == 0) {
						sql.Clear();
					}

					foreach (var field in changed_fields) {
						sql.Append(field.Key).Append(" = ").Append(BindParameter(field.Value)).Append(", ");
					}

					sql.Remove(sql.Length - 2, 2).AppendLine();
					break;
				case Mode.Delete:
					sql.Append("DELETE FROM ").AppendLine(table_name);
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

				if (sql_limit_offset != -1) {
					sql.Append(sql_limit_offset).Append(", ");
				}

				sql.Append(sql_limit_count);

			}

			command.CommandText = sql.ToString();
			if (context.Debug.HasFlag(Context.DebugLevel.Queries)) {
				Console.Out.WriteLine("Query: \r\n" + command.CommandText);
			}
		}


		/// <summary>
		/// Binds a parameter in the current command.
		/// </summary>
		/// <param name="value">Value to bind.</param>
		/// <returns>Parameter name for the binding reference.</returns>
		private string BindParameter(object value, List<DbParameter> parameter_list = null) {
			string key = "@p" + command.Parameters.Count;
			var param = command.CreateParameter();
			param.ParameterName = key;
			param.Value = value;

			// Logging to output bound parameters to stdout.
			if (context.Debug.HasFlag(Context.DebugLevel.BoundParameters)) {
				Console.Out.WriteLine("Parameter: " + key + " = " + value.ToString());
			}

			if (parameter_list != null) {
				parameter_list.Add(param);
			}

			command.Parameters.Add(param);
			return key;
		}

		/// <summary>
		/// Inserts multiple rows into the database.
		/// </summary>
		/// <remarks>
		/// This method by default wraps all inserts into a transaction.
		/// If one of the inserts fails, then all of the inserts are rolled back.
		/// </remarks>
		/// <param name="models">Rows to insert.</param>
		public long[] Insert(T[] models) {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (models == null || models.Length == 0) {
				throw new ArgumentException("Model array is empty.");
			}

			if (mode != Mode.Insert) {
				throw new InvalidOperationException("Can not insert when statement is not in INSERT mode.");
			}

			var columns = models[0].GetColumns();

			StringBuilder sb_sql = new StringBuilder();
			sb_sql.Append("INSERT INTO ").Append(table_name).Append(" (");

			// Add all the column names.
			foreach (var column in columns) {
				sb_sql.Append("").Append(column).Append(", ");
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
			sb_sql.Append(");");

			long[] new_row_ids = null;

			if (context.LastInsertIdQuery != null) {
				sb_sql.Append(context.LastInsertIdQuery);
				new_row_ids = new long[models.Length];
			}
			DtxTransaction transaction = null;

			try {

				// Start a transaction if one does not already exist for fast bulk inserts.
				if (context.TransactionStarted == false) {
					transaction = context.BeginTransaction();
				}

				command.CommandText = sb_sql.ToString();

				// Create the parameters for bulk inserts.
				for (int i = 0; i < columns.Length; i++) {
					var parameter = command.CreateParameter();
					parameter.ParameterName = "@v" + i;
					command.Parameters.Add(parameter);
				}

				// Loop through watch of the provided models.
				for (int i = 0; i < models.Length; i++) {
					var values = models[i].GetAllValues();

					for (int x = 0; x < values.Length; x++) {
						command.Parameters[x].Value = values[x];
					}

					if (context.LastInsertIdQuery != null) {
						object new_row = command.ExecuteScalar();
						if (new_row == null) {
							throw new Exception("Unable to insert row");
						} else {
							new_row_ids[i] = Convert.ToInt64(new_row);
						}
					} else {
						if (command.ExecuteNonQuery() != 1) {
							throw new Exception("Unable to insert row");
						}
					}


				}

				// Commit all inserts.
				if (transaction != null) {
					transaction.Commit();
				}
			} catch (Exception e) {
				// If we encountered an error, rollback the transaction.

				if (transaction != null) {
					transaction.Rollback();
				}

				throw;
			} finally {
				if (transaction != null) {
					transaction.Dispose();
				}
			}
			return new_row_ids;

		}


		/// <summary>
		/// Binds the specified parameters to the partial SQL statement.
		/// </summary>
		/// <param name="sql">SQL to bind the parameters to.</param>
		/// <param name="binding">Objects to bind to the partial SQL statement.</param>
		/// <returns>Formatted SQL string to put into the final SQL query.</returns>
		private string SqlBindParameters(string sql, object[] binding) {
			if (binding == null) {
				return sql;
			}

			string[] sql_param_holder = new string[binding.Length];
			for (int i = 0; i < binding.Length; i++) {
				sql_param_holder[i] = BindParameter(binding[i]);
			}

			try {
				return string.Format(sql, sql_param_holder);
			} catch (Exception e) {
				throw new Exception("Invalid number of placement parameters for the WHERE statement.", e);
			}
		}

		/// <summary>
		/// Validates the current state of the class and checks to see if a where statement is allowed to be called.
		/// </summary>
		private void ValidateWhere() {
			if (mode == Mode.Execute) {
				throw new InvalidOperationException("Can not use all functions in Execute mode.");
			}

			if (mode == Mode.Insert) {
				throw new InvalidOperationException("Can not use the WHERE method in INSERT mode.");
			}

			if (sql_where != null) {
				throw new InvalidOperationException("The WHERE statement has already been defined.");
			}
		}

		public void Dispose() {
			this.command.Dispose();
		}

	}
}
