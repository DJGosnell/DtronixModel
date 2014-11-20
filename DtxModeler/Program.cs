using DtxModeler.Generator.CodeGen;
using DtxModeler.Ddl;
using DtxModeler.Generator.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DtxModeler.Xaml;
using System.Windows;
using System.Runtime.InteropServices;
using NDesk.Options;
using DtxModeler.Generator.MySqlMwb;

namespace DtxModeler.Generator {
	class Program {

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		const int SW_HIDE = 0;
		const int SW_SHOW = 5;

		public static bool CommandlineVisible { get; private set; }

		[STAThread]
		static void Main(string[] args) {

			// Get and hide the console and show the UI if there are no arguments passed.
			if (args.Length == 0) {
				CommandlineHide();
				// Show the UI and start the main loop.
				var app = new Application();
				app.Run(new MainWindow());
				return;
			}

			CommandOptions options = new CommandOptions(args, Console.Out);


			// Verify that the parsing was successful.
			if (options.ParseSuccess == false) {
				return;
			}

			try {
				Task.Run(async () => {
					await ExecuteOptions(options, null);
				}).Wait();				
			} catch (AggregateException e) {
				Console.WriteLine("Error in executing specified options. Error:");
				foreach (var ex in e.InnerExceptions) {
					Console.WriteLine(ex.Message);
				}
				
			}

		}

		public static void CommandlineShow() {
			ShowWindow(GetConsoleWindow(), SW_SHOW);
			CommandlineVisible = true;
		}

		public static void CommandlineHide() {
			ShowWindow(GetConsoleWindow(), SW_HIDE);
			CommandlineVisible = false;
		}

		public static async Task ExecuteOptions(CommandOptions options, Database input_database) {
			DdlGenerator generator = null;
			TypeTransformer type_transformer = new SqliteTypeTransformer();
			bool rooted_path = Path.IsPathRooted(options.SqlOutput);

			if (options.InputType == CommandOptions.InType.Ddl) {
				if (input_database == null) {
					try {
						using (FileStream stream = new FileStream(options.Input, FileMode.Open)) {
							var serializer = new XmlSerializer(typeof(Database));
							input_database = (Database)serializer.Deserialize(stream);
						}
					} catch (Exception e) {
						writeLineColor("Could not open input DDL file at '" + options.Input + "'.", ConsoleColor.Red);
						writeLineColor(e.ToString(), ConsoleColor.Red);
						return;
					}
				} else {
					Console.WriteLine("Using database passed.");
				}

			} else if (options.InputType == CommandOptions.InType.DatabaseSqlite) {
				generator = new SqliteDdlGenerator(@"Data Source=" + options.Input + ";Version=3;");

				input_database = await generator.GenerateDdl();
			} else if (options.InputType == CommandOptions.InType.Mwb) {

				if (File.Exists(options.Input) == false) {
					throw new OptionException("MWB file '" + options.Input + "' specified does not exist.", "input");
				}


				generator = new MySqlMwbDdlGenerator(options.Input);

				input_database = await generator.GenerateDdl();
			}

			// Ensure that the base database is initialized.
			input_database.Initialize();

			// Overrides for database variables.
			if (string.IsNullOrWhiteSpace(options.Namespace) == false) {
				input_database.Namespace = options.Namespace;
			}

			if (string.IsNullOrWhiteSpace(options.ContextClass) == false) {
				input_database.ContextClass = options.ContextClass;
			}
			

			// Clean up the DDL
			if (normalizeDatabase(input_database) == false) {
				writeLineColor("Could not normalize input database.", ConsoleColor.Red);
				return;
			}

			// Output SQL file if required.
			if (options.SqlOutput != null) {
				var sql_code_writer = new SqlDatabaseGen(input_database, type_transformer);

				if (options.SqlOutput == "") {
					options.SqlOutput = Path.GetFileNameWithoutExtension(input_database.Name);
				}

				if (Path.HasExtension(options.SqlOutput) == false) {
					options.SqlOutput = Path.ChangeExtension(options.SqlOutput, ".sql");
				}
				using (var fs = new FileStream(options.SqlOutput, FileMode.Create)) {
					using (var sw = new StreamWriter(fs)) {
						sw.Write(sql_code_writer.generate());
						sw.Flush();
					}
				}
			}

			// Output code if required.
			if (options.CodeOutput != null) {
				var table_code_writer = new TableModelGen(input_database);
				var database_code_writer = new DatabaseContextGen(input_database);

				if (options.CodeOutput == "") {
					options.CodeOutput = Path.ChangeExtension(Path.GetFileNameWithoutExtension(input_database.Name), ".cs");
				}

				if (Path.HasExtension(options.CodeOutput) == false) {
					options.CodeOutput = Path.ChangeExtension(options.CodeOutput, ".cs");
				}

				// Output code file if required.
				using (var fs = new FileStream(options.CodeOutput, FileMode.Create)) {
					using (var sw = new StreamWriter(fs)) {
						sw.Write(database_code_writer.Generate());

						foreach (var db_table in input_database.Table) {
							sw.Write(table_code_writer.generate(db_table));
						}

						// Final closing namespace
						sw.Write("}");
						sw.Flush();

					}
				}
			}

			// Output Ddl if required.
			if (options.DdlOutput != null) {
				if (options.DdlOutput == "") {
					options.DdlOutput = Path.GetFileNameWithoutExtension(input_database.Name);
				}

				if (Path.HasExtension(options.DdlOutput) == false) {
					options.DdlOutput = Path.ChangeExtension(options.DdlOutput, ".ddl");
				}

				// Output code file if required.
				using (var fs = new FileStream(options.DdlOutput, FileMode.Create)) {
					var serializer = new XmlSerializer(typeof(Database));
					serializer.Serialize(fs, input_database);
				}
			}
		}

		private static bool normalizeDatabase(Database database) {
			Dictionary<string, Association> associations = new Dictionary<string, Association>();

			foreach (var table in database.Table) {

				foreach (var column in table.Column) {
					var is_name_null = string.IsNullOrWhiteSpace(column.Name);
					var is_member_null = string.IsNullOrWhiteSpace(column.Name);

					// If the column name is empty, then assume that the database column name
					// is the same as the member name and vice versa.
					if (is_member_null && is_name_null) {
						writeLineColor("Column on table " + table.Name + "Does not have a name.", ConsoleColor.Red);
						return false;
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
