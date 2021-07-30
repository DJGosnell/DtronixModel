using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DtronixModeler.Generator;
using DtronixModeler.Generator.Ddl;
using Column = DtronixModeler.Generator.Ddl.Column;
using Database = DtronixModeler.Generator.Ddl.Database;
using Index = DtronixModeler.Generator.Ddl.Index;
using Table = DtronixModeler.Generator.Ddl.Table;

namespace DtronixModeler.Sqlite
{
    public class SqliteDdlGenerator : DdlGenerator
    {

        List<Configuration> configurations = new List<Configuration>();

        public SqliteDdlGenerator(string connection_string)
            : base(new SqliteTypeTransformer())
        {
            connection = new SQLiteConnection(connection_string);
        }

        public SqliteDdlGenerator(SQLiteConnection connection)
            : base(new SqliteTypeTransformer())
        {
            this.connection = connection;
        }

        private void GetPragmaConfiguration(DbCommand command, string query, string config_name, Func<DbDataReader, string> config_value, string description)
        {

        }

        public override async Task<Database> GenerateDdl()
        {
            await Task.Run(() => {
                connection.Open();
                database.Name = Path.GetFileNameWithoutExtension(connection.DataSource);

                // Get the tables.
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT tbl_name FROM sqlite_master WHERE type = 'table'";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var table = new Table();
                            table.Name = reader.GetString(0);

                            if (table.Name == "sqlite_sequence")
                            {
                                continue;
                            }
                            database.Table.Add(table);
                        }
                    }
                }

                // Get default Pragma configurations.
                using (var command = connection.CreateCommand())
                {
                    foreach (var config in SqlitePragmaConfigurations.Configurations)
                    {
                        string name = config.Name.Replace("sqlite.pragma.", "");

                        command.CommandText = "PRAGMA " + name;

                        using var reader = command.ExecuteReader();
                        reader.Read();
                        try
                        {
                            database.Configuration.Add(new Configuration()
                            {
                                Name = config.Name,
                                Value = reader[name].ToString(),
                                Description = config.Description
                            });
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }

                var tables = database.Table.ToArray();
                foreach (var table in tables)
                {
                    // Get the columns
                    using (var command = connection.CreateCommand())
                    {
                        try
                        {
                            command.CommandText = "PRAGMA table_info(" + table.Name + ")";
                            using var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                try
                                {
                                    var typ = reader["type"].ToString();

                                    // If the type can't be read, then treat it as a text columns.
                                    if (typ == "")
                                        typ = "TEXT";

                                    var column = new Column();
                                    column.Name = reader["name"].ToString();
                                    column.IsPrimaryKey = Convert.ToBoolean(reader["pk"]);
                                    column.Nullable = !Convert.ToBoolean(reader["notnull"]);
                                    column.DbType = typ;
                                    column.NetType = TypeTransformer.DbToNetType(typ, false);

                                    if (column.IsPrimaryKey && column.DbType.ToLower() == "integer")
                                    {
                                        column.IsPrimaryKey = true;
                                    }

                                    table.Column.Add(column);
                                }
                                catch
                                {
                                    // Do nothing as the column can't be read.  Should probably log...
                                }

                            }
                        }
                        catch
                        {
                            database.Table.Remove(table);
                            // If an exception is thrown while trying to read the table_info, just remove the 
                            // table completely.  Log...
                        }
                
                    }

                    // Get the indexes.

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT name, tbl_name FROM sqlite_master WHERE type = 'index' AND tbl_name = @TableName";
                        Utilities.addDbParameter(command, "@TableName", table.Name);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var index = new Index()
                                {
                                    Name = reader.GetString(0),
                                    Table = Enumerable.First<Table>(database.Table, t => t.Name == table.Name)
                                };

                                table.Index.Add(index);
                            }
                        }
                    }

                    if (table.Index.Count == 0)
                    {
                        continue;
                    }

                    using (var command = connection.CreateCommand())
                    {

                        var columns = new List<IndexColumn>();

                        foreach (var index in table.Index)
                        {
                            columns.Clear();
                            // Using a parameter here instead of a hard value seems to cause SQL syntax errors...
                            command.CommandText = "PRAGMA index_info ( " + index.Name + " )";

                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    index.IndexColumn.Add(new IndexColumn()
                                    {
                                        Name = reader["name"].ToString()
                                    });
                                }
                            }
                        }
                    }
                }

                //CREATE UNIQUE INDEX "main"."Test" ON "MangaTitles" ("Manga_id" ASC)
                connection.Close();
            });

            return database;

        }
    }
}
