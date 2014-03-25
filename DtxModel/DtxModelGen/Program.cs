using DtxModelGen.CodeGen;
using DtxModelGen.Schema.Dbml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DtxModelGen {
	class Program {
		static void Main(string[] args) {

			var options = new CommandOptions();
			Database database = null;

			var parser = new CommandLine.Parser((CommandLine.ParserSettings settings) => {
				settings.HelpWriter = Console.Out;
			});
			var parse_result = parser.ParseArguments(args, options);

			if(options.CodeOutput == null){
				
			}

			// Verify that the parsing was successful.
			if (parse_result == false) {
				writeColor("Invalid input parameters.", ConsoleColor.Red);
				return;
			}

			using (FileStream stream = new FileStream(options.DbmlInput, FileMode.Open)) {
				var serializer = new XmlSerializer(typeof(Database));
				database = (Database)serializer.Deserialize(stream);
			}

			if (options.SqlOutput != null) {
				//var sql_writer = new SqlWriter(database);
				//sql_writer.WriteTo(options.SqlOutput);
			}

			if (normalizeDatabase(database) == false) {
				return;
			}

			var table_code_writer = new TableModelGen();
			var database_code_writer = new DatabaseContextGen();
			database_code_writer.DatabaseName = database;
			database_code_writer.Ns = options.CodeNamespace;

			table_code_writer.Transformer = new Sqlite.SqliteTypeTransformer();
			table_code_writer.Ns = options.CodeNamespace;

			using (var fs = new FileStream(options.CodeOutput, FileMode.Create)) {
				using (var sw = new StreamWriter(fs)) {
					sw.Write(database_code_writer.generate());

					foreach (var db_table in database.Table) {
						table_code_writer.DbTable = db_table;
						sw.Write(table_code_writer.generate());
						
					}

					// Final closing namespace
					sw.Write("}");
					sw.Flush();
					
				}
			}

			

		}

		private static bool normalizeDatabase(Database database) {
			foreach (var table in database.Table) {
				foreach (var item in table.Type.Items) {
					if (item is Column) {
						var column = item as Column;
						var is_name_null = string.IsNullOrWhiteSpace(column.Name);
						var is_member_null = string.IsNullOrWhiteSpace(column.Member);
						// If the column name is empty, then assume that the database column name
						// is the same as the member name and vice versa.
						if (is_name_null) {
							column.Name = column.Member;

						} else if (is_member_null) {
							column.Member = column.Name;

						} else if (is_member_null && is_name_null) {
							writeColor("Column on table " + table.Name + "Does not have a name or member.", ConsoleColor.Red);
							return false;
						}

						// Default values.
						if (column.IsReadOnlySpecified == false) {
							column.IsReadOnly = false;
						}

						if (column.IsDbGeneratedSpecified == false) {
							column.IsDbGenerated = false;
						}

						if (column.IsPrimaryKeySpecified == false) {
							column.IsPrimaryKey = false;
						}
					}
				}
			}

			return true;
		}


		public static void writeColor(string text, ConsoleColor color) {
			var original_color = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.Write(text);
			Console.ForegroundColor = original_color;
		}

		public static void writeLineColor(string text, ConsoleColor color) {
			var original_color = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine(text);
			Console.ForegroundColor = original_color;
		}
	}
}
