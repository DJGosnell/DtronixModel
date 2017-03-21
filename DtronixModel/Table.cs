
namespace DtronixModel {

	/// <summary>
	/// Generic class to wrap individual table models.
	/// </summary>
	/// <typeparam name="T">Table model to wrap.</typeparam>
	public class Table<T> where T : Model, new() {
		private readonly Context context;

		/// <summary>
		/// Constructor to define which context this table is operating inside.
		/// </summary>
		/// <param name="context">Context of the current database.</param>
		public Table(Context context) {
			this.context = context;
		}

		/// <summary>
		/// Begins a select query. Ensure to dispose of SqlStatement
		/// </summary>
		/// <param name="select"></param>
		/// <returns>SqlStatement to chain up following query modifiers.</returns>
		public SqlStatement<T> Select(string select = "*") {
			return new SqlStatement<T>(SqlStatement<T>.Mode.Select, context).Select(select);
		}

		/// <summary>
		/// Inserts a single row into the database.
		/// </summary>
		/// <param name="model">Row to insert.</param>
		/// <returns>If the "LastInsertIdQuery" property is set on the database context, will return the newly inserted row id.  Otherwise will return 0.</returns>
		public long Insert(T model) {
			long[] result = Insert(new[] { model });
			return result?[0] ?? 0;
		}

		/// <summary>
		/// Inserts multiple rows into the database inside a transaction.
		/// </summary>
		/// <remarks>
		/// Since all of the inserts are performed inside a transaction, if one of the inserts fail, all of the inserts rollback.
		/// </remarks>
		/// <param name="model">Row to insert.</param>
		/// <returns>If the "LastInsertIdQuery" property is set on the database context, will return the newly inserted row ids in a long[].  Otherwise will return null.</returns>
		public long[] Insert(T[] model) {
			using (var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Insert, context)) {
				return statement.Insert(model);
            }
        }

		/// <summary>
		/// Updates a single row in the database. Must have its primary key set.
		/// </summary>
		/// <remarks>
		/// The primary key (Usually the row id) has to be set for the function to determine which row to update.
		/// </remarks>
		/// <param name="model">Row to update.</param>
		public void Update(T model) {
			Update(new[] { model });
		}

		/// <summary>
		/// Updates multiple rows in the database. Must have their primary keys set.
		/// </summary>
		/// <remarks>
		/// The primary key (Usually the row id) has to be set for the function to determine which row to update.
		/// </remarks>
		/// <param name="model">Rows to update.</param>
		public void Update(T[] model) {
			using (var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Update, context)) {
				statement.Update(model);
			}
		}

		/// <summary>
		/// Deletes a row from the database. Must have its primary key set.
		/// </summary>
		/// <remarks>
		/// The primary key (Usually the row id) has to be set for the function to determine which row to update.
		/// </remarks>
		/// <param name="model">Row to delete.</param>
		public void Delete(T model) {
			Delete(new[] { model });
		}

		/// <summary>
		/// Deletes multiple rows in the database. Must have their primary keys set.
		/// </summary>
		/// <param name="models">Rows to delete.</param>
		public void Delete(T[] models) {
			using (var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Delete, context)) {
				statement.Delete(models);
			}
		}

		/// <summary>
		/// Deletes a single row in the database based upon the primary row id.
		/// </summary>
		/// <param name="id">Row id to delete.</param>
		public void Delete(long id) {
			Delete(new[] { id });
		}

		/// <summary>
		/// Delete multiple rows in the database based upon the primary row ids.
		/// </summary>
		/// <param name="ids">Row ids to delete.</param>
		public void Delete(long[] ids) {
			using (var statement = new SqlStatement<T>(SqlStatement<T>.Mode.Delete, context)) {
				statement.Delete(ids);
			}
		}
	}
}
