using DtxModeler.Ddl;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace DtxModeler.Generator.Sqlite {
	public class SqliteDdlGenerator : DdlGenerator {

		List<Configuration> configurations = new List<Configuration>();

		public SqliteDdlGenerator(string connection_string)
			: base(new SqliteTypeTransformer()) {
			connection = new SQLiteConnection(connection_string);
		}

		public SqliteDdlGenerator(SQLiteConnection connection)
			: base(new SqliteTypeTransformer()) {
				this.connection = connection;
		}

		private void SetConfiguration(DbCommand command, string query, string config_name, Func<DbDataReader, string> config_value) {
			command.CommandText = query;

			using (var reader = command.ExecuteReader()) {
				reader.Read();
				try {
					database.Configuration.Add(new Configuration() {
						Name = config_name,
						Value = config_value(reader)
					});
				} catch { }

			}
		}

		public override Database generateDdl() {
			connection.Open();
			database.Name = Path.GetFileNameWithoutExtension(connection.DataSource);

			// Get the tables.
			using (var command = connection.CreateCommand()) {
				command.CommandText = "SELECT tbl_name FROM sqlite_master WHERE type = 'table'";
				using (var reader = command.ExecuteReader()) {
					while (reader.Read()) {
						var table = new Table();
						table.Name = reader.GetString(0);

						if (table.Name == "sqlite_sequence") {
							continue;
						}
						database.Table.Add(table);
					}
				}
			}

			// Get default Pragma configurations.
			using (var command = connection.CreateCommand()) {


				
				SetConfiguration(command, "PRAGMA auto_vacuum", "sqlite.pragma.auto_vacuum", reader => reader["auto_vacuum"].ToString());
				SetConfiguration(command, "PRAGMA automatic_index", "sqlite.pragma.automatic_index", reader => reader["automatic_index"].ToString());
				SetConfiguration(command, "PRAGMA busy_timeout", "sqlite.pragma.busy_timeout", reader => reader["busy_timeout"].ToString());
				SetConfiguration(command, "PRAGMA cache_size", "sqlite.pragma.cache_size", reader => reader["cache_size"].ToString());
				SetConfiguration(command, "PRAGMA cache_spill", "sqlite.pragma.cache_spill", reader => reader["automatic_index"].ToString());
				SetConfiguration(command, "PRAGMA checkpoint_fullfsync", "sqlite.pragma.checkpoint_fullfsync", reader => reader["checkpoint_fullfsync"].ToString());
				SetConfiguration(command, "PRAGMA defer_foreign_keys", "sqlite.defer_foreign_keys", reader => reader["defer_foreign_keys"].ToString());
				SetConfiguration(command, "PRAGMA encoding", "sqlite.pragma.encoding", reader => reader["encoding"].ToString());
				SetConfiguration(command, "PRAGMA foreign_key_check", "sqlite.pragma.foreign_key_check", reader => reader["foreign_key_check"].ToString());
				SetConfiguration(command, "PRAGMA foreign_keys", "sqlite.pragma.foreign_keys", reader => reader["foreign_keys"].ToString());
				SetConfiguration(command, "PRAGMA full_column_names", "sqlite.pragma.full_column_names", reader => reader["full_column_names"].ToString());

				SetConfiguration(command, "PRAGMA fullfsync", "sqlite.pragma.fullfsync", reader => reader["fullfsync"].ToString());
				SetConfiguration(command, "PRAGMA integrity_check", "sqlite.pragma.integrity_check", reader => reader["integrity_check"].ToString());
				SetConfiguration(command, "PRAGMA journal_mode", "sqlite.pragma.journal_mode", reader => reader["journal_mode"].ToString());
				SetConfiguration(command, "PRAGMA journal_size_limit", "sqlite.pragma.journal_size_limit", reader => reader["journal_size_limit"].ToString());
				SetConfiguration(command, "PRAGMA legacy_file_format", "sqlite.pragma.legacy_file_format", reader => reader["legacy_file_format"].ToString());
				SetConfiguration(command, "PRAGMA locking_mode", "sqlite.pragma.locking_mode", reader => reader["locking_mode"].ToString());
				SetConfiguration(command, "PRAGMA max_page_count", "sqlite.pragma.max_page_count", reader => reader["max_page_count"].ToString());
				SetConfiguration(command, "PRAGMA page_size", "sqlite.pragma.page_size", reader => reader["page_size"].ToString());
				SetConfiguration(command, "PRAGMA query_only", "sqlite.pragma.query_only", reader => reader["query_only"].ToString());
				SetConfiguration(command, "PRAGMA quick_check", "sqlite.pragma.quick_check", reader => reader["quick_check"].ToString());
				SetConfiguration(command, "PRAGMA read_uncommitted", "sqlite.pragma.read_uncommitted", reader => reader["read_uncommitted"].ToString());
				SetConfiguration(command, "PRAGMA recursive_triggers", "sqlite.pragma.recursive_triggers", reader => reader["recursive_triggers"].ToString());
				SetConfiguration(command, "PRAGMA reverse_unordered_selects", "sqlite.pragma.reverse_unordered_selects", reader => reader["reverse_unordered_selects"].ToString());
				SetConfiguration(command, "PRAGMA schema_version", "sqlite.pragma.schema_version", reader => reader["schema_version"].ToString());
				SetConfiguration(command, "PRAGMA user_version", "sqlite.pragma.user_version", reader => reader["user_version"].ToString());
				SetConfiguration(command, "PRAGMA secure_delete", "sqlite.pragma.secure_delete", reader => reader["secure_delete"].ToString());
				SetConfiguration(command, "PRAGMA soft_heap_limit", "sqlite.pragma.soft_heap_limit", reader => reader["soft_heap_limit"].ToString());
				SetConfiguration(command, "PRAGMA synchronous", "sqlite.pragma.synchronous", reader => reader["synchronous"].ToString());
				SetConfiguration(command, "PRAGMA temp_store", "sqlite.pragma.temp_store", reader => reader["temp_store"].ToString());
				SetConfiguration(command, "PRAGMA wal_autocheckpoint", "sqlite.pragma.wal_autocheckpoint", reader => reader["wal_autocheckpoint"].ToString());
			}

			foreach (var table in database.Table) {
				// Get the columns
				using (var command = connection.CreateCommand()) {
					command.CommandText = "PRAGMA table_info(" + table.Name + ")";
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

							table.Column.Add(column);
						}
					}
				}

				// Get the indexes.

				using (var command = connection.CreateCommand()) {
					command.CommandText = "SELECT name, tbl_name FROM sqlite_master WHERE type = 'index' AND tbl_name = @TableName";
					Utilities.addDbParameter(command, "@TableName", table.Name);

					using (var reader = command.ExecuteReader()) {
						while (reader.Read()) {
							var index = new Index() {
								Name = reader.GetString(0),
								Table = getTableByName(table.Name)
							};

							table.Index.Add(index);
						}
					}
				}

				if (table.Index.Count == 0) {
					continue;
				}

				using (var command = connection.CreateCommand()) {

					var columns = new List<IndexColumn>();

					foreach (var index in table.Index) {
						columns.Clear();
						// Using a parameter here instead of a hard value seems to cause SQL syntax errors...
						command.CommandText = "PRAGMA index_info ( " + index.Name + " )";

						using (var reader = command.ExecuteReader()) {
							while (reader.Read()) {
								index.IndexColumn.Add(new IndexColumn() {
									Name = reader["name"].ToString()
								});
							}
						}
					}
				}
			}

			//CREATE UNIQUE INDEX "main"."Test" ON "MangaTitles" ("Manga_id" ASC)
			connection.Close();

			return database;

		}
	}
}
