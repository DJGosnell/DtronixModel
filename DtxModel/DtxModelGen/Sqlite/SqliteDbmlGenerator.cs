using DtxModelGen.Schema.Ddl;
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
		private Database database;

		public SqliteDbmlGenerator(string connection_string, TypeTransformer type_transformer) {
			connection = new SQLiteConnection(connection_string);
			connection.Open();
			this.type_transformer = type_transformer;
			database = new Database();
		}

		public Database generateDatabase() {
			database.Name = Path.ChangeExtension(connection.DataSource, "");
			database.Table = getTables();
			var table_items = new List<object>();

			foreach (var table in database.Table) {
				table_items.Clear();

				getTableColumns(table_items, table.Name);
				getIndexes(table_items, table.Name);

				table.Type = new Schema.Ddl.Type() {
					Name = table.Name
				};

				table.Type.Items = table_items.ToArray();
				
			}

			//CREATE UNIQUE INDEX "main"."Test" ON "MangaTitles" ("Manga_id" ASC)
			return database;
		}

		private Table getTableByName(string name) {
			foreach (var table in database.Table) {
				if (table.Name == name) {
					return table;
				}
			}

			return null;
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

		public void getIndexes(List<object> items_list, string table_name) {
			List<Index> indexes = new List<Index>();
			using (var command = connection.CreateCommand()) {
				command.CommandText = "SELECT name, tbl_name FROM sqlite_master WHERE type = 'index' AND tbl_name = @TableName";
				command.Parameters.AddWithValue("@TableName", table_name);

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
				return;
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

				items_list.AddRange(indexes);
			}
		}

		public void getTableColumns(List<object> items_list, string table) {

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

			items_list.AddRange(columns);
		}
	}
}
