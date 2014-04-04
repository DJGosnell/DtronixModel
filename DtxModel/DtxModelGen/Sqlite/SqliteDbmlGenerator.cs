using DtxModelGen.Schema.Dbml;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace DtxModelGen.Sqlite {
	public class SqliteDbmlGenerator {
		private SQLiteConnection connection;
		private TypeTransformer type_transformer;

		public SqliteDbmlGenerator(string connection_string, TypeTransformer type_transformer) {
			connection = new SQLiteConnection(connection_string);
			connection.Open();
			this.type_transformer = type_transformer;
		}

		public Database generateDatabase() {
			var database = new Database();
			database.Name = Path.ChangeExtension(connection.DataSource, "");
			database.Table = getTables();

			foreach (var table in database.Table) {
				var columns = getTableColumns(table.Name);
				table.Type = new Schema.Dbml.Type() {
					Name = table.Name
				};
				table.Type.Items = columns;
			}


			return database;
		}

		public Table[] getTables() {
			List<Table> tables = new List<Table>();
			using (var command = connection.CreateCommand()) {
				command.CommandText = "SELECT tbl_name FROM sqlite_master WHERE type = 'table'";
				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						var table = new Table();
						table.Name = reader.GetString(0);
						table.Member = table.Name;

						if (table.Name == "sqlite_sequence") {
							continue;
						}
						tables.Add(table);
					}
				}
			}

			return tables.ToArray();
		}

		public Column[] getTableColumns(string table) {

			List<Column> columns = new List<Column>();
			using (var command = connection.CreateCommand()) {
				command.CommandText = "PRAGMA table_info(" + table + ")";
				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						var typ = reader["type"].ToString();
						object vals = reader.GetValues();

						var column = new Column() {
							Member = reader["name"].ToString(),
							Name = reader["name"].ToString(),
							IsPrimaryKey = Convert.ToBoolean(reader["pk"]),
							IsPrimaryKeySpecified = true,
							CanBeNull = !Convert.ToBoolean(reader["notnull"]),
							CanBeNullSpecified = true,
							DbType = reader["type"].ToString(),
							Type = type_transformer.dbToNetType(reader["type"].ToString()),
						};

						if (column.IsPrimaryKey && column.DbType.ToLower() == "integer") {
							column.IsDbGenerated = true;
							column.IsDbGeneratedSpecified = true;
						}

						columns.Add(column);
					}
				}
			}

			return columns.ToArray();
		}
	}
}
