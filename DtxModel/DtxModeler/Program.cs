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

namespace DtxModeler.Generator {
	class Program {

		[STAThread]
		static void Main(string[] args) {

			var options = new ModelGenOptions(args);
			Database input_database = null;
			DdlGenerator generator = null;

			if (options.UI) {
				var app = new Application();
				app.Run(new MainWindow());
				return;
			}


			// Verify that the parsing was successful.
			if (options.ParseSuccess == false) {
				writeLineColor("Invalid input parameters.", ConsoleColor.Red);
				return;
			}

			if (options.DbType.ToLower() == "sqlite") {
				generator = new SqliteDdlGenerator(@"Data Source=" + options.Input + ";Version=3;");
			}

			if (options.InputType == "ddl") {
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
			} else if (options.InputType == "database") {

				if (options.DbType.ToLower() == "sqlite") {
					generator = new SqliteDdlGenerator(@"Data Source=" + options.Input + ";Version=3;");
				}

				if (options.DbClass == null) {
					writeLineColor("Required 'db-class' attribute not selected.", ConsoleColor.Red);
				}

				input_database = generator.generateDdl();
			}


			// Clean up the DDL
			if (normalizeDatabase(input_database) == false) {
				writeLineColor("Could not normalize input database.", ConsoleColor.Red);
				return;
			}

			

			// Output SQL file if required.
			if (options.SqlOutput != null) {
				var sql_code_writer = new SqlDatabaseGen(input_database, generator.TypeTransformer);

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

				// Output code file if required.
				using (var fs = new FileStream(options.CodeOutput, FileMode.Create)) {
					using (var sw = new StreamWriter(fs)) {
						sw.Write(database_code_writer.generate());

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

				// Member / Name.
				if (string.IsNullOrWhiteSpace(table.Name) && string.IsNullOrWhiteSpace(table.Member) == false) {
					table.Name = table.Member;
				}

				foreach (var column in table.Column) {
					var is_name_null = string.IsNullOrWhiteSpace(column.Name);
					var is_member_null = string.IsNullOrWhiteSpace(column.Member);

					column.Table = table;

					// If the column name is empty, then assume that the database column name
					// is the same as the member name and vice versa.
					if (is_name_null) {
						column.Name = column.Member;

					} else if (is_member_null) {
						column.Member = column.Name;

					} else if (is_member_null && is_name_null) {
						writeLineColor("Column on table " + table.Name + "Does not have a name or member.", ConsoleColor.Red);
						return false;
					}

					// Default values.
					/*if (column.IsReadOnlySpecified == false) {
						column.IsReadOnly = false;
					}

					if (column.IsDbGeneratedSpecified == false) {
						column.IsDbGenerated = false;
					}

					if (column.IsPrimaryKeySpecified == false) {
						column.IsPrimaryKey = false;
					}*/
				}


				foreach (var association in table.Association ?? new Association[0]) {
					association.Table = table;

					// Determine if we already have this association in the list
					if (associations.ContainsKey(association.Name)) {
						var other = associations[association.Name];

						if (other.IsForeignKey) {
							// This is the child table
							other.ParentAssociation = association;
							association.ChildAssociation = other;
						} else {
							// This is the parent association.
							other.ChildAssociation = association;
							association.ParentAssociation = other;
						}

						// Get the colums for the associations.
						Column this_column = null;
						Column other_column = null;

						// Get the association's corrisponding columns
						foreach (var assoc_col in association.Table.Column) {
							if (assoc_col.Member == association.ThisKey) {
								this_column = assoc_col;
								break;
							}
						}

						foreach (var assoc_col in other.Table.Column) {
							if (assoc_col.Member == association.OtherKey) {
								other_column = assoc_col;
								break;
							}
						}

						association.OtherKeyColumn = other.ThisKeyColumn = other_column;
						association.ThisKeyColumn = other.OtherKeyColumn = this_column;

						associations.Remove(association.Name);

					} else {
						// Add the association to the list for later.
						associations.Add(association.Name, association);
					}

					/*
					if (association.IsForeignKeySpecified == false) {
						association.IsForeignKey = false;
					}*/

					if (association.CardinalitySpecified == false && association.IsForeignKey == false) {
						association.Cardinality = Cardinality.Many;
					} else {
						association.Cardinality = Cardinality.One;
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
			Console.Read();
		}
	}
}
