using DtronixModel.Generator.Ddl;

namespace DtronixModel.Generator.Output {
	public class SqlTableCreateGenerator {
		private Database database;
		private TypeTransformer type_transformer;
		private CodeWriter code = new CodeWriter();

		public SqlTableCreateGenerator(Database database, TypeTransformer type_transformer) { 

			this.database = database;
			this.type_transformer = type_transformer;
		}

		/// <summary>
		/// Generates and returns the SQL CREATE TABLE statements along with the indexes to generate the database.
		/// </summary>
		/// <returns>SQL string representing the table.</returns>
		public string Generate() {
			code.clear();

			// Loop through each of the tables.
			foreach (var table in database.Table) {

				if (table.UseCustomSql) {
					code.WriteLine("/* Custom Table SQL */");
					code.WriteLine(table.CustomSql).WriteLine().WriteLine();

				} else {
					code.BeginBlock("CREATE TABLE ").Write((string) table.Name).WriteLine(" (");

					// Columns
					foreach (var column in table.Column) {
						string net_type = type_transformer.NetToDbType(column.NetType);

						code.Write((string) column.Name).Write(" ").Write(net_type).Write(" ");

						if (column.Nullable == false) {
							code.Write("NOT NULL ");
						}

						if (column.IsPrimaryKey) {
							code.Write("PRIMARY KEY ");
						}

						if (column.IsAutoIncrement) {
							code.Write("AUTOINCREMENT ");
						}

						code.WriteLine(",");
					}

					code.removeLength(1);

					code.EndBlock(");").WriteLine();
				}

				// Indexes
				
				foreach (var column in table.Column) {
					if (column.DbType != null && column.DbType.Contains("IDX")) {
						code.Write("CREATE INDEX IF NOT EXISTS IDX_").Write((string) table.Name).Write("_").Write((string) column.Name)
                            .Write(" ON ").Write((string) table.Name).Write(" (").Write((string) column.Name).WriteLine(");");
					}

				}

				code.WriteLine().WriteLine();
			}

		    code.Write((string) database.CreationSql).WriteLine();

			return code.ToString();
		}
	}
}
