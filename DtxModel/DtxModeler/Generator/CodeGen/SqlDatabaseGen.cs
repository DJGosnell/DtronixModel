using DtxModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator.CodeGen {
	class SqlDatabaseGen : CodeGenerator {

		public SqlDatabaseGen(Database database, TypeTransformer type_transformer) : base(database) {
			this.type_transformer = type_transformer;
		}

		public string generate() {
			code.clear();

			foreach (var table in database.Table) {
				code.beginBlock("CREATE TABLE ").write(table.Name).writeLine(" (");

				// Columns
				foreach (var column in table.Column) {
					string net_type = type_transformer.NetToDbType(column.Type);

					code.write(column.Name).write(" ").write(net_type).write(" ");

					if(column.Nullable == false) {
						code.write("NOT NULL ");
					}

					if (column.IsPrimaryKey) {
						code.write("PRIMARY KEY ");
					}

					if (column.IsAutoIncrementSpecified && column.IsAutoIncrement) {
						code.write("AUTOINCREMENT ");
					}

					code.writeLine(",");
				}

				code.removeLength(1);

				code.endBlock(");").writeLine().writeLine();

				// Indexes
				
				foreach (var column in table.Column) {
					if (column.DbType != null && column.DbType.Contains("IDX")) {
						code.write("CREATE INDEX IF NOT EXISTS IDX_").write(table.Name).write("_").write(column.Name)
							.write(" ON ").write(table.Name).write(" (").write(column.Name).writeLine(");");
					}

				}

				code.writeLine().writeLine();
			}

			return code.ToString();
		}
	}
}
