using DtxModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator.Sqlite {
	public class SqliteDdlGenerator : DdlGenerator {


		public SqliteDdlGenerator(string connection_string) : base(null, new SqliteTypeTransformer()) {
			connection = new SQLiteConnection(connection_string);
		}

		public override Database generateDdl() {
			connection.Open();
			database.Name = Path.GetFileNameWithoutExtension(connection.DataSource);
			database.Table = getTables();
			foreach (var table in database.Table) {

				table.Column = getTableColumns(table.Name);
				table.Index = getIndexes(table.Name);
			}

			//CREATE UNIQUE INDEX "main"."Test" ON "MangaTitles" ("Manga_id" ASC)
			return database;
		}

		public override Table[] getTables() {
			List<Table> tables = new List<Table>();
			using (var command = connection.CreateCommand()) {
				command.CommandText = "SELECT tbl_name FROM sqlite_master WHERE type = 'table'";
				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						var table = new Table();
						table.Name = reader.GetString(0);

						if (table.Name == "sqlite_sequence") {
							continue;
						}
						tables.Add(table);
					}
				}
			}

			return tables.ToArray();
		}

		public override Index[] getIndexes(string table_name) {
			List<Index> indexes = new List<Index>();
			using (var command = connection.CreateCommand()) {
				command.CommandText = "SELECT name, tbl_name FROM sqlite_master WHERE type = 'index' AND tbl_name = @TableName";
				Utilities.addDbParameter(command, "@TableName", table_name);

				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						var index = new Index() {
							Name = reader.GetString(0),
							Table = getTableByName(table_name)
						};

						indexes.Add(index);
					}
				}
			}

			if (indexes.Count == 0) {
				return null;
			}

			using (var command = connection.CreateCommand()) {
				
				var columns = new List<IndexColumn>();

				foreach (var index in indexes) {
					columns.Clear();
					// Using a parameter here instead of a hard value seems to cause SQL syntax errors...
					command.CommandText = "PRAGMA index_info ( " + index.Name + " )";

					using (var reader = command.ExecuteReader()) {
						while (reader.Read()) {
							columns.Add(new IndexColumn() {
								Name = reader["name"].ToString()
							});
						}
					}

					index.IndexColumn = columns.ToArray();
				}

				return indexes.ToArray();
			}
		}

		public override Column[] getTableColumns(string table) {

			List<Column> columns = new List<Column>();
			using (var command = connection.CreateCommand()) {
				command.CommandText = "PRAGMA table_info(" + table + ")";
				using (var reader = command.ExecuteReader()) {

					
					while (reader.Read()) {
						var typ = reader["type"].ToString();

						var column = new Column() {
							Name = reader["name"].ToString(),
							IsPrimaryKey = Convert.ToBoolean(reader["pk"]),
							Nullable = !Convert.ToBoolean(reader["notnull"]),
							DbType = reader["type"].ToString(),
							NetType = TypeTransformer.DbToNetType(reader["type"].ToString()),
						};

						if (column.IsPrimaryKey && column.DbType.ToLower() == "integer") {
							column.IsPrimaryKey = true;
						}

						columns.Add(column);
					}
				}
			}

			return columns.ToArray();
		}
	}
}
