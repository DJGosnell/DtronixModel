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

		}


		public static void writeColor(string text, ConsoleColor color) {
			Console.ForegroundColor = color;
			Console.Write(text);
		}

		public static void writeLineColor(string text, ConsoleColor color) {
			writeColor(text, color);
			Console.WriteLine();
		}
	}
}
