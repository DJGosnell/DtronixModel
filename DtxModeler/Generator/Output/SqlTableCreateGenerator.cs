using DtxModeler.Ddl;
using DtxModeler.Generator.MySql;
using DtxModeler.Generator.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator.Output {
	class SqlTableCreateGenerator {
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
					code.BeginBlock("CREATE TABLE ").Write(table.Name).WriteLine(" (");

					// Columns
					foreach (var column in table.Column) {
						string net_type = type_transformer.NetToDbType(column.NetType);

						code.Write(column.Name).Write(" ").Write(net_type).Write(" ");

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

					code.EndBlock(");").WriteLine().WriteLine();
				}

				// Indexes
				
				foreach (var column in table.Column) {
					if (column.DbType != null && column.DbType.Contains("IDX")) {
						code.Write("CREATE INDEX IF NOT EXISTS IDX_").Write(table.Name).Write("_").Write(column.Name)
							.Write(" ON ").Write(table.Name).Write(" (").Write(column.Name).WriteLine(");");
					}

				}

				code.WriteLine().WriteLine();
			}

			return code.ToString();
		}
	}
}
