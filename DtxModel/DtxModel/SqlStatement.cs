using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Linq;
using System.Reflection;

namespace DtxModel {
	class ExpressionToSql : ExpressionVisitor {
		public StringBuilder sb = new StringBuilder();
		private int parameter_index;
		private DbCommand command;

		public ExpressionToSql(DbCommand command) {
			this.command = command;
		}

		protected override Expression VisitBinary(BinaryExpression node) {
			sb.Append("(");

			this.Visit(node.Left);

			switch (node.NodeType) {
				case ExpressionType.GreaterThanOrEqual:
					sb.Append(" >= ");
					break;

				case ExpressionType.LessThanOrEqual:
					sb.Append(" <= ");
					break;

				case ExpressionType.GreaterThan:
					sb.Append(" > ");
					break;

				case ExpressionType.LessThan:
					sb.Append(" < ");
					break;

				case ExpressionType.Equal:
					sb.Append(" == ");
					break;

				case ExpressionType.AndAlso:
					sb.Append(" && ");
					break;

				case ExpressionType.Or:
					sb.Append(" | ");
					break;

				case ExpressionType.OrElse:
					sb.Append(" || ");
					break;

				case ExpressionType.Add:
					sb.Append(" + ");
					break;

				case ExpressionType.Divide:
					sb.Append(" / ");
					break;
			}

			this.Visit(node.Right);

			sb.Append(")");

			return node;
		}

		protected override Expression VisitMember(MemberExpression node) {
			var s = this;
			//sb.Append(node.Member.ReflectedType.Name).Append(".");
			var atts = node.Member.GetCustomAttributes(typeof(System.Data.Linq.Mapping.ColumnAttribute), false);
			if (atts.Length != 0) {
				// This is just a column.  Add its name and continue.
				sb.Append(((System.Data.Linq.Mapping.ColumnAttribute)atts[0]).Name);
			} else {
				/*// This is a field or property.  Get its value and bind it.
				int param_index = parameter_index++;

				sb.Append("@p").Append(param_index);

				// Bind the parameter
				var param = command.CreateParameter();
				param.ParameterName = "@p" + param_index.ToString();

				if (node.Expression is ConstantExpression) {
					var inner = (ConstantExpression)node.Expression;
					param.Value = (node.Member as FieldInfo).GetValue(inner.Value);
				}

				
				//param.Value = n*/
			}
			
			return node;
		}

		protected override Expression VisitConstant(ConstantExpression node) {
			int param_index = parameter_index++;
			sb.Append("@p").Append(param_index);

			// Bind the parameter
			var param = command.CreateParameter();
			param.ParameterName = "@p" + param_index.ToString();
			param.Value = node.Value;

			command.Parameters.Add(param);

			return node;
		}
	}



	public class SqlStatement<T> {
		public enum Mode {
			Select,
			Insert,
			Update,
			Delete
		}

		public DbConnection connection;
		public DbCommand command;

		public static SqlStatement<T> Select () {
			return new SqlStatement<T>(Mode.Select);
		}

		public static SqlStatement<T> Update {
			get {
				return new SqlStatement<T>(Mode.Update);
			}
		}

		public static SqlStatement<T> Insert {
			get {
				return new SqlStatement<T>(Mode.Insert);
			}
		}
        
		public static SqlStatement<T> Delete {
			get {
				return new SqlStatement<T>(Mode.Delete);
			}
		}


		private Mode mode;

		public SqlStatement(Mode mode) : this(mode, null) { }

		public SqlStatement(Mode mode, DbConnection connection) {
			this.connection = connection;
			this.mode = mode;
			command = connection.CreateCommand();

		}

		public SqlStatement<T> where(Expression<Func<T, bool>> expression){



			StringBuilder sql = new StringBuilder();
			/*ExpressionType.be

			var ops = new Dictionary<ExpressionType, String>();
        ops.Add(ExpressionType.Equal, "=");
        ops.Add(ExpressionType.GreaterThan, ">");*/

			Expression current_expression = expression.Body;

			var expv = new ExpressionToSql(command);
			expv.Visit(expression);


			string test = "";

			return this;
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
			var columns = models[0].getColumns();
			var sql = buildInsertStatement(table, columns);

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
