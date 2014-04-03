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

			var table_code_writer = new TableModelGen(database);
			var database_code_writer = new DatabaseContextGen(database);
			var type_transformer = new Sqlite.SqliteTypeTransformer();

			if (options.SqlOutput != null) {
				var sql_code_writer = new SqlDatabaseGen(database, type_transformer);

				using (var fs = new FileStream(options.SqlOutput, FileMode.Create)) {
					using (var sw = new StreamWriter(fs)) {
						sw.Write(sql_code_writer.generate());
						sw.Flush();
					}
				}
			}

			using (var fs = new FileStream(options.CodeOutput, FileMode.Create)) {
				using (var sw = new StreamWriter(fs)) {
					sw.Write(database_code_writer.generate());

					foreach (var db_table in database.Table) {
						sw.Write(table_code_writer.generate(db_table));
					}

					// Final closing namespace
					sw.Write("}");
					sw.Flush();
					
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

				foreach (var item in table.Type.Items) {
					if (item is Column) {
						var column = item as Column;
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
							writeColor("Column on table " + table.Name + "Does not have a name or member.", ConsoleColor.Red);
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

					}else if (item is Association) {
						var association = item as Association;
						association.Table = table;

						// Determine if we already have this association in the list
						if (associations.ContainsKey(association.Name)) {
							var other = associations[association.Name];
							
							if (other.IsForeignKeySpecified && other.IsForeignKey) {
								// This is the child table
								other.ParentAssociation = association;
								association.ChildAssociation = other;
							}else {
								// This is the parent association.
								other.ChildAssociation = association;
								association.ParentAssociation = other;
							}

							// Get the colums for the associations.
							Column this_column = null;
							Column other_column = null;

							// Get the association's corrisponding columns
							Utilities.each<Column>(association.Table.Type.Items, assoc_col => {
								if (assoc_col.Member == association.ThisKey) {
									this_column = assoc_col;
									return false;
								}
								return true;
							});

							Utilities.each<Column>(other.Table.Type.Items, assoc_col => {
								if (assoc_col.Member == association.OtherKey) {
									other_column = assoc_col;
									return false;
								}
								return true;
							});

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
